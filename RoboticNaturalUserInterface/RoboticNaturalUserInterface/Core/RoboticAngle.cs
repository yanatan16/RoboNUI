using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.Core
{
    /**
     * Robotic Angles
     * 
     * List of angles possible in this system
     * Each servo controller will only use a subset of these
     */
    enum RoboticAngle
    {
        // Human-like motions
        NeckRotation,
        NeckTilt,
        RightShoulderTilt,
        LeftShouderTilt,
        RightElbow,
        LeftElbow,
        RightWrist,
        LeftWrist,
        WaistRotation,
        RightLegTilt,
        LeftLegTilt,
        RightKnee,
        LeftKnee,
        RightAnkle,
        LeftAnkle,

        //Special motions
        BaseRotation,
        CurtainOpen
    }
}
