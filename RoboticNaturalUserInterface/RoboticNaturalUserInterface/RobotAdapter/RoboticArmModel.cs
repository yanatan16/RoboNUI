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

        /**
         * <summary>Construct the Robotic Arm Model</summary>
         * 
         */
        public RoboticArmModel()
        {
            NeededJoints = new List<ControllerJoints>();
            NeededJoints.Add(ControllerJoints.ShoulderCenter);
            NeededJoints.Add(ControllerJoints.ShoulderRight);
            NeededJoints.Add(ControllerJoints.ElbowRight);
            NeededJoints.Add(ControllerJoints.WristRight);
            NeededJoints.Add(ControllerJoints.HandRight);
        }


        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            AngleSet angles = new AngleSet();

            Position3d r = js.JointMap[ControllerJoints.ElbowRight] - js.JointMap[ControllerJoints.ShoulderRight];
            
            angles.AngleMap.Add(RoboticAngle.ArmBaseRotate,
                Math.Atan2(r.y, r.x) + Math.PI / 2
            );
            angles.AngleMap.Add(RoboticAngle.ArmShoulderLift,
                - Math.Atan2(r.z * 10, r.x)
            );
            angles.AngleMap.Add(RoboticAngle.ArmElbowBend,
                getAngle3d(
                    js.JointMap[ControllerJoints.ShoulderRight],
                    js.JointMap[ControllerJoints.ElbowRight],
                    js.JointMap[ControllerJoints.WristRight]
                ) - Math.PI
            );
            angles.AngleMap.Add(RoboticAngle.ArmWristTilt, 
                getAngle3d(
                    js.JointMap[ControllerJoints.ElbowRight],
                    js.JointMap[ControllerJoints.WristRight],
                    js.JointMap[ControllerJoints.HandRight]
                )
            );

            //TODO Figure out these angles
            angles.AngleMap.Add(RoboticAngle.ArmWristRotate, 0);
            angles.AngleMap.Add(RoboticAngle.ArmHandGrasp, 0);

            log.Debug("Elbow vector: " + (js.JointMap[ControllerJoints.ElbowRight] - js.JointMap[ControllerJoints.ShoulderRight]).ToString());
            log.Debug("Incoming JointSet: " + js.ToString());
            log.Debug("Translating JointSet to AngleSet: (" + angles.ToString() + ")");

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
