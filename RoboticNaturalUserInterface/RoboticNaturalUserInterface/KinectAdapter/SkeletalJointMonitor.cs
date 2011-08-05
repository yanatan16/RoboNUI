using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNUI.Core;

namespace RoboNUI.KinectAdapter
{
    /**
     * Skeletal Joint Monitor
     * 
     * This class monitors the Kinect data feed for updates to the Joint 
     * positions for certain monitored joints. The joints in question are 
     * passed in dynamically. After a certain period, also passed in dynamically,
     * the new Joint positions are forwarded to the JAT.
     */
    class SkeletalJointMonitor
    {
        private JointAngleTranslator jat;
        private double period;
        //private Joints interestedJoints;

        public SkeletalJointMonitor(JointAngleTranslator _jat)
        {
            this.jat = _jat;
            period = 0;
        }

        public void setPeriod(double _period)
        {
            this.period = _period;
        }

        public double getPeriod()
        {
            return period;
        }

//        public void setJointList(Joints _joints)
//        {
//            interestedJoints = _joints;
//        }
    }
}
