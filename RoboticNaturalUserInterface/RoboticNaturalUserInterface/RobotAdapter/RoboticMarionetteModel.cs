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

        double avghipcenter = 0;

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
            avghipcenter = 0;
        }

        /**
         * <summary>Reset the robotic marionette</summary>
         * <see cref="IRoboticModel.Reset"/>
         */
        public AngleSet Reset()
        {
            log.Debug("RESET!");
            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.HeadLift, 0);
            angles.AngleMap.Add(RoboticAngle.LeftArmLift, 0);
            angles.AngleMap.Add(RoboticAngle.RightArmLift, 0);
            angles.AngleMap.Add(RoboticAngle.RearLift, 0);
            angles.AngleMap.Add(RoboticAngle.CurtainOpen, Math.PI);
            avghipcenter = 0;
            return angles;
        }

        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            double hipdiff = js.JointMap[ControllerJoints.HipCenter].y - avghipcenter;
            avghipcenter = (avghipcenter + js.JointMap[ControllerJoints.HipCenter].y) / 2;

            // Find the height above for each node
            Position3d lefthand = js.JointMap[ControllerJoints.HandLeft] - js.JointMap[ControllerJoints.ShoulderLeft];
            Position3d righthand = js.JointMap[ControllerJoints.HandRight] - js.JointMap[ControllerJoints.ShoulderRight];

            double handLeftAngle = -Math.Atan2(lefthand.y, -lefthand.x);
            double handRightAngle = -Math.Atan2(righthand.y, righthand.x);

            Position3d hip = js.JointMap[ControllerJoints.HipCenter];
            Position3d head = js.JointMap[ControllerJoints.Head];
            double dely = head.y - hip.y;
            double delz = Math.Max(0,hip.z - head.z);
            double bendAngle = Math.Atan2(delz, dely);
            
            double bendPct = Math.PI/4 - (bendAngle - Math.PI/2) / (Math.PI/2);
            double headAngle = bendAngle * Math.PI * 2 - Math.PI - hipdiff;
            double rearAngle = -headAngle - hipdiff;

            log.ErrorFormat("handRight: {0}", handRightAngle);
            
            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.HeadLift, headAngle);
            angles.AngleMap.Add(RoboticAngle.LeftArmLift, handLeftAngle);
            angles.AngleMap.Add(RoboticAngle.RightArmLift, handRightAngle);
            angles.AngleMap.Add(RoboticAngle.RearLift, rearAngle);
            angles.AngleMap.Add(RoboticAngle.CurtainOpen, -Math.PI);

            //log.Debug("Translating JointSet to AngleSet: as(" + angles.ToString() + ")");

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
