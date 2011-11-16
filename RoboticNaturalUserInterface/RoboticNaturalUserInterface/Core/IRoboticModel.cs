using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Provides the capability to translate from a human model of Joints to a Robotic model of Angles
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     */
    interface IRoboticModel
    {
        /**
        * <summary> The list of joints that this model requires to translate.</summary>
        */
        List<ControllerJoints> NeededJoints { get; }

        /**
         * <summary>
         * Translate between a human Joints model to a Robotic angle model
         * </summary>
         * <param name="js"> Joint positions from human controller </param>
         * <returns> Robotic angles from robot model </returns>
         */
        AngleSet Translate(JointSet js);

        /**
         * <summary>
         * Reset the angle set (as if no controller in the view)
         * </summary>
         * <returns> Robotic angles from robot model </returns>
         */
        AngleSet Reset();
    }
}
