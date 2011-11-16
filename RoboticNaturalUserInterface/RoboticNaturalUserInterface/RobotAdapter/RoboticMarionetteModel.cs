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
     * This class provides a translation from human controller joints to Robotic Marionette angles.
     * These angles are determined by a model of the physical robotic marionette itself.
     * </summary>
     * 
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * <seealso cref="IRoboticModel"/>
     */
    class RoboticMarionetteModel : IRoboticModel
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RoboticArmModel)); 
        /**
        * <summary> The list of joints that this model requires to translate.</summary>
        */
        public List<ControllerJoints> NeededJoints { get; private set; }

        List<double> heights;

        /**
         * <summary>Construct the Robotic Marionette Model</summary>
         * 
         */
        public RoboticMarionetteModel()
        {
            heights = new List<double>();

            NeededJoints = new List<ControllerJoints>();
            NeededJoints.Add(ControllerJoints.Head);
            NeededJoints.Add(ControllerJoints.HandLeft);
            NeededJoints.Add(ControllerJoints.ShoulderLeft);
            NeededJoints.Add(ControllerJoints.ShoulderRight);
            NeededJoints.Add(ControllerJoints.HandRight);
            NeededJoints.Add(ControllerJoints.HipCenter);
            NeededJoints.Add(ControllerJoints.KneeLeft);
            NeededJoints.Add(ControllerJoints.KneeRight);
        }

        /**
         * <summary>Reset the robotic marionette</summary>
         * <see cref="IRoboticModel.Reset"/>
         */
        public AngleSet Reset()
        {
            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.HeadLift, 0);
            angles.AngleMap.Add(RoboticAngle.LeftArmLift, 0);
            angles.AngleMap.Add(RoboticAngle.RightArmLift, 0);
            angles.AngleMap.Add(RoboticAngle.RearLift, 0);
            return angles;
        }

        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            // Find the height above for each node
            Position3d lefthand = js.JointMap[ControllerJoints.HandLeft] - js.JointMap[ControllerJoints.ShoulderLeft];
            Position3d righthand = js.JointMap[ControllerJoints.HandRight] - js.JointMap[ControllerJoints.ShoulderRight];

            double handLeftAngle = -Math.Atan2(lefthand.y, -lefthand.x);
            double handRightAngle = -Math.Atan2(righthand.y, righthand.x);

            Position3d legs = (2 * js.JointMap[ControllerJoints.HipCenter] - js.JointMap[ControllerJoints.KneeLeft] - js.JointMap[ControllerJoints.KneeRight]) / 2;
            Position3d torso = -js.JointMap[ControllerJoints.HipCenter] - js.JointMap[ControllerJoints.Head];
            //legs.x = 0;
            //torso.x = 0;
            double bendAngle = legs.angle(torso) - Math.PI / 2;
            double headAngle = bendAngle * 4;
            double rearAngle = -headAngle;

            log.Debug("legs: " + legs.ToString());
            log.Debug("torso: " + torso.ToString());
            log.Debug("bendAngle(" + bendAngle + ") headAngle(" + headAngle + ")");
            
            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.HeadLift, headAngle);
            angles.AngleMap.Add(RoboticAngle.LeftArmLift, handLeftAngle);
            angles.AngleMap.Add(RoboticAngle.RightArmLift, handRightAngle);
            angles.AngleMap.Add(RoboticAngle.RearLift, rearAngle);
            
            log.Debug("Translating JointSet to AngleSet: as(" + angles.ToString() + ")");

            return angles;
        }

        private double getAndAddToAverageHeight(double toAdd)
        {
            double sum = 0;
            foreach (double d in heights)
                sum += d;
            sum /= heights.Count;
            heights.Add(toAdd);
            if (heights.Count > 10)
                heights.RemoveAt(0);
            return sum;
        }
    }
}
