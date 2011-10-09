using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

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
            NeededJoints.Add(ControllerJoints.ShoulderRight);
        }


        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            AngleSet angles = new AngleSet();

            Position3d flatShoulder = js.JointMap[ControllerJoints.ShoulderRight];
            flatShoulder.z = 0;
            angles.AngleMap.Add(RoboticAngle.ArmBaseRotate,
                getAngle3d(
                    flatShoulder,
                    js.JointMap[ControllerJoints.ShoulderCenter],
                    js.JointMap[ControllerJoints.ShoulderRight]
                )
            );
            angles.AngleMap.Add(RoboticAngle.ArmShoulderLift, 
                getAngle3d(
                    js.JointMap[ControllerJoints.ShoulderCenter],
                    js.JointMap[ControllerJoints.ShoulderRight],
                    js.JointMap[ControllerJoints.ElbowRight]
                )
            );
            angles.AngleMap.Add(RoboticAngle.ArmElbowBend,
                getAngle3d(
                    js.JointMap[ControllerJoints.ShoulderRight],
                    js.JointMap[ControllerJoints.ElbowRight],
                    js.JointMap[ControllerJoints.WristRight]
                )
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

            return angles;
        }

        private double getAngle3d(Position3d a, Position3d b, Position3d c)
        {
            Position3d vec1 = a - b;
            Position3d vec2 = c - b;
            return Math.Acos(vec1.Dot(vec2) / (vec1.Magnitude() * vec2.Magnitude()));
        }
    }
}
