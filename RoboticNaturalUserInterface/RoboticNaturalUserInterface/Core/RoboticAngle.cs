using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Newtonsoft.Json.Converters;

namespace RoboNui.Core
{
    /**
     * <summary>
     * List of angles possible in this system
     * </summary>
     * <remarks>
     * Each servo controller will only use a subset of these
     * </remarks>
     */
    public enum RoboticAngle
    {
        // Angles for Arm

        /// <summary> Arm angle of the arm with respect to a horizontal rotation about the base</summary>
        ArmBaseRotate,
        /// <summary> Arm angle of the vertical tilt of the arm</summary>
        ArmShoulderLift,
        /// <summary> Arm angle of the elbow's bend</summary>
        ArmElbowBend,
        /// <summary> Arm angle of the wrist's tilt</summary>
        ArmWristTilt,
        /// <summary> Arm angle of the wrist's rotation</summary>
        ArmWristRotate,
        /// <summary> Arm angle of the claw's grasp</summary>
        ArmHandGrasp,

        
        // Angles for Marionette

        /// <summary> Marionette angle of the head's lift </summary>
        HeadLift,
        /// <summary> Marionette angle of the right arm's lift </summary>
        RightArmLift,
        /// <summary> Marionette angle of the left arm's lift </summary>
        LeftArmLift,
        /// <summary> Marionette angle of the lift of the rear of the Marionette </summary>
        RearLift,
        /// <summary> Marionette angle of the curtain's open location </summary>
        CurtainOpen,

        Length

    }
}
