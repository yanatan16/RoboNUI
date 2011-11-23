using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * <summary>
     * Enumeration of human controller joints possible in this system
     * Each <see cref="IRoboticModel"/> will only need a subset of these
     * </summary>
     * 
     * <remarks>
     * This is an exact copy of JointID from Microsoft.Research.Kinect.Nui and 
     * is done so to enable non-Kinect SDK users to develop for this system
     * </remarks>
     * <seealso cref="Microsoft.Research.Kinect.Nui.JointID"/>
     * <seealso cref="IRoboticModel"/>
     * <seealso cref="JointSet"/>
     */
    public enum ControllerJoints
    {
        HipCenter = 0,
        Spine = 1,
        ShoulderCenter = 2,
        Head = 3,
        ShoulderLeft = 4,
        ElbowLeft = 5,
        WristLeft = 6,
        HandLeft = 7,
        ShoulderRight = 8,
        ElbowRight = 9,
        WristRight = 10,
        HandRight = 11,
        HipLeft = 12,
        KneeLeft = 13,
        AnkleLeft = 14,
        FootLeft = 15,
        HipRight = 16,
        KneeRight = 17,
        AnkleRight = 18,
        FootRight = 19,

        //HandTracker
        Fingertip, //Largest fingertip from palm
        Fingerstart, //Largest finger start

        //Count
        Count,
    }
}
