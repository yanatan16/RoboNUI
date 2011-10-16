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

        /**
         * <summary>Construct the Robotic Marionette Model</summary>
         * 
         */
        public RoboticMarionetteModel()
        {
            NeededJoints = new List<ControllerJoints>();
            NeededJoints.Add(ControllerJoints.Head);
            NeededJoints.Add(ControllerJoints.HandLeft);
            NeededJoints.Add(ControllerJoints.HandRight);
            NeededJoints.Add(ControllerJoints.HipCenter);
            NeededJoints.Add(ControllerJoints.FootLeft);
            NeededJoints.Add(ControllerJoints.FootRight);
        }


        /**
         * <summary>Translate between joints to angles for the robotic arm</summary>
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            // TODO Figure out how to scale these
            const double scale = .5;
            const double constant = -1;

            // Find the floor level
            double averageFloor = (js.JointMap[ControllerJoints.FootLeft].y + js.JointMap[ControllerJoints.FootRight].y) / 2;

            // Find the height above for each node
            double headHeight = js.JointMap[ControllerJoints.Head].y - averageFloor;
            double handLeftHeight = js.JointMap[ControllerJoints.Head].y - averageFloor;
            double handRightHeight = js.JointMap[ControllerJoints.Head].y - averageFloor;
            double hipHeight = js.JointMap[ControllerJoints.Head].y - averageFloor;

            AngleSet angles = new AngleSet();
            angles.AngleMap.Add(RoboticAngle.HeadLift, headHeight * scale + constant);
            angles.AngleMap.Add(RoboticAngle.LeftArmLift, handLeftHeight * scale + constant);
            angles.AngleMap.Add(RoboticAngle.RightArmLift, handRightHeight * scale + constant);
            angles.AngleMap.Add(RoboticAngle.RearLift, hipHeight * scale + constant);
            
            log.Debug("Translating JointSet to AngleSet: js(" + js.ToString() + ") to as(" + angles.ToString() + ")");

            return angles;
        }
    }
}
