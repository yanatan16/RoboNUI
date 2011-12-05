using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

using log4net;

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class provides a translation from human controller joints to Robotic Arm angles.
     * These angles are determined by a model of the physical robotic arm itself.
     * </summary>
     * 
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * <seealso cref="IRoboticModel"/>
     */
    class RoboticArmModel : IRoboticModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RoboticArmModel)); 
        /**
        * <summary> The list of joints that this model requires to translate.</summary>
        */
        public List<ControllerJoints> NeededJoints { get; private set; }

        public bool UseRightArm
        {
            get { return (NeededJoints == neededJointsRight); }
            set { if (value) NeededJoints = neededJointsRight; else NeededJoints = neededJointsLeft; }
        }

        private List<ControllerJoints> neededJointsRight;
        private List<ControllerJoints> neededJointsLeft;
        private Position3d keptHand, keptTopArm;

        /**
         * <summary>Construct the Robotic Arm Model</summary>
         * 
         */
        public RoboticArmModel()
        {
            neededJointsRight = new List<ControllerJoints>();
            neededJointsRight.Add(ControllerJoints.ShoulderCenter);
            neededJointsRight.Add(ControllerJoints.ShoulderRight);
            neededJointsRight.Add(ControllerJoints.ElbowRight);
            neededJointsRight.Add(ControllerJoints.WristRight);
            neededJointsRight.Add(ControllerJoints.HandRight);
            neededJointsRight.Add(ControllerJoints.Fingertip);
            neededJointsRight.Add(ControllerJoints.Fingertip);

            neededJointsLeft = new List<ControllerJoints>();
            neededJointsLeft.Add(ControllerJoints.ShoulderCenter);
            neededJointsLeft.Add(ControllerJoints.ShoulderLeft);
            neededJointsLeft.Add(ControllerJoints.ElbowLeft);
            neededJointsLeft.Add(ControllerJoints.WristLeft);
            neededJointsLeft.Add(ControllerJoints.HandLeft);
            neededJointsLeft.Add(ControllerJoints.Fingertip);
            neededJointsLeft.Add(ControllerJoints.Fingertip);

            UseRightArm = true;

            keptHand = new Position3d();
            keptTopArm = new Position3d();
        }

        /**
         * <summary>Reset the robotic arm</summary>
         * <see cref="IRoboticModel.Reset"/>
         */
        public AngleSet Reset()
        {
            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.ArmBaseRotate, 0);
            angles.AngleMap.Add(RoboticAngle.ArmShoulderLift, 0);
            angles.AngleMap.Add(RoboticAngle.ArmElbowBend, 0);
            angles.AngleMap.Add(RoboticAngle.ArmWristTilt, 0);
            angles.AngleMap.Add(RoboticAngle.ArmWristRotate, 0);
            angles.AngleMap.Add(RoboticAngle.ArmHandGrasp, 0);
            return angles;
        }

        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            AngleSet angles = new AngleSet();

            ControllerJoints Elbow = (UseRightArm ? ControllerJoints.ElbowRight : ControllerJoints.ElbowLeft);
            ControllerJoints Shoulder = (UseRightArm ? ControllerJoints.ShoulderRight : ControllerJoints.ShoulderLeft);
            ControllerJoints Wrist = (UseRightArm ? ControllerJoints.WristRight : ControllerJoints.WristLeft);
            ControllerJoints Hand = (UseRightArm ? ControllerJoints.HandRight : ControllerJoints.HandLeft);

            // Shoulder joints
            if (js.JointMap.ContainsKey(Elbow) &&
                js.JointMap.ContainsKey(Shoulder) &&
                js.JointMap.ContainsKey(Wrist) &&
                js.JointMap.ContainsKey(Hand))
            {
                Position3d topArm = js.JointMap[Elbow] - js.JointMap[Shoulder];
                angles.AngleMap.Add(RoboticAngle.ArmBaseRotate,
                    Math.Atan2(topArm.z, topArm.x)
                );
                angles.AngleMap.Add(RoboticAngle.ArmShoulderLift,
                    Math.Atan2(topArm.y, topArm.x)
                );

                Position3d bottomArm = js.JointMap[Wrist] - js.JointMap[Elbow];
                double elbowangle = Math.Acos(bottomArm.Dot(topArm) / (topArm.Magnitude() * bottomArm.Magnitude()));
                double elbowsign = (UseRightArm ? -1 : 1) * Math.Sign(topArm.Cross(bottomArm).z);
                double elbowshift = -Math.PI / 4;
                angles.AngleMap.Add(RoboticAngle.ArmElbowBend,
                    elbowangle * elbowsign + elbowshift
                );

                Position3d hand = js.JointMap[Hand] - js.JointMap[Wrist];
                double wristangle = Math.Acos(bottomArm.Dot(hand) / (hand.Magnitude() * bottomArm.Magnitude()));
                double wristsign = (UseRightArm ? 1 : -1) * Math.Sign(bottomArm.Cross(hand).z);
                double wristshift = Math.PI / 6;
                double wristmult = 2;
                angles.AngleMap.Add(RoboticAngle.ArmWristTilt,
                    (wristangle * wristsign + wristshift) * wristmult
                );

                keptHand = hand;
                keptTopArm = topArm;
            }

            if (js.JointMap.ContainsKey(Hand) &&
                js.JointMap.ContainsKey(ControllerJoints.Fingertip) &&
                js.JointMap.ContainsKey(ControllerJoints.Fingerstart))
            {
                Position3d fingertip = js.JointMap[ControllerJoints.Fingertip];
                Position3d fingerstart = js.JointMap[ControllerJoints.Fingerstart];
                Position3d hand = js.JointMap[Hand];
                double open = (fingertip - fingerstart).Magnitude();
                double fingerscale = 1;

                Position3d realperp = (fingertip - hand).Cross(fingerstart - hand);
                Position3d imagperp = keptHand.Cross(keptTopArm);
                double rotateangle = Math.Acos(realperp.Dot(imagperp) / (realperp.Magnitude() * imagperp.Magnitude()));
                double rotatesign = Math.Sign(keptHand.Dot(realperp.Cross(imagperp)));
                double rotatescale = 1;

                angles.AngleMap.Add(RoboticAngle.ArmWristRotate, rotateangle * rotatesign * rotatescale);
                angles.AngleMap.Add(RoboticAngle.ArmHandGrasp, open * fingerscale);
            }

            return angles;
        }

        private double getAngle3d(Position3d a, Position3d b, Position3d c)
        {
            Position3d vec1 = a - b;
            Position3d vec2 = c - b;
            return Math.Acos(vec1.Dot(vec2) / (vec1.Magnitude() * vec2.Magnitude()));
        }

        private double getProjectedAngle(Position3d a, Position3d b, Position3d c, Dimension toProject)
        {
            Position3d ap = new Position3d(a);
            Position3d bp = new Position3d(b);
            Position3d cp = new Position3d(c);

            if (toProject == Dimension.X)
            {
                ap.x = 0;
                bp.x = 0;
                cp.x = 0;
            }
            else if (toProject == Dimension.Y)
            {
                ap.y = 0;
                bp.y = 0;
                cp.y = 0;
            }
            else
            {
                ap.z = 0;
                bp.z = 0;
                cp.z = 0;
            }
            return getAngle3d(ap, bp, cp);
        }

        private double getProjectedPolarTheta(Position3d center, Position3d point, Dimension toProject)
        {
            Position3d r = point - center;

            if (toProject == Dimension.X)
                return Math.Atan2(r.z, r.y);
            else if (toProject == Dimension.Y)
                return Math.Atan2(r.z, r.x);
            else
                return Math.Atan2(r.y, r.x);
        }
    }
}
