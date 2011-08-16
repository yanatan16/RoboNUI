using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

using Utilities.Messaging;

using Microsoft.Research.Kinect.Nui;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class monitors the Kinect data feed for updates to the Joint positions for certain monitored joints. 
     * The joints in question are passed in dynamically.
     * After a certain period, also passed in dynamically, the new Joint positions are forwarded to the JAT.
     * </summary>
     * 
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     * 
     */
    class SkeletalJointMonitor : Provider<JointSet>
    {
        /**
         * <summary>
         * The time period between retrieval of the Kinect joints
         * </summary>
         */
        public double Period { get; set; }

        /**
         * <summary> The list of joints the SJM should forward every <see cref="F:Period"/>.</summary>
         */
        public List<JointID> InterestedJoints { get; set; }

        /**
         * <summary>Nui runtime for communicating with the Kinect</summary>
         */
        private Runtime nui;


        /**
         * <summary> Basic Constructor </summary>
         */
        public SkeletalJointMonitor() :
            base()
        {
            Period = 0;
            InterestedJoints = new List<JointID>();

            nui = new Runtime(); 
            try
            {
                nui.Initialize(RuntimeOptions.UseDepthAndPlayerIndex | RuntimeOptions.UseSkeletalTracking);
            }
            catch (InvalidOperationException)
            {
                //TODO Log this: ("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                return;
            }
        }



    }
}
