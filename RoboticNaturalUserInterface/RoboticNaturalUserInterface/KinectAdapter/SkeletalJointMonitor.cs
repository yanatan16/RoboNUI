using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

using Messaging;

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
     */
    class SkeletalJointMonitor : Provider<JointSet>
    {
        //TODO Implement

        public double Period { get; set; }

        public List<JointID> InterestedJoints { get; set; }

        public SkeletalJointMonitor() :
            base()
        {
            Period = 0;
            InterestedJoints = new List<JointID>();
        }

        public void setPeriod(double _period)
        {
            this.Period = _period;
        }

        public double getPeriod()
        {
            return Period;
        }

//        public void setJointList(Joints _joints)
//        {
//            interestedJoints = _joints;
//        }

    }
}
