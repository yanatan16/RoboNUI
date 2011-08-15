using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
