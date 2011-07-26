using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI
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
        }

        public void setPeriod(double _period)
        {
            this.period = _period;
        }

//        public void setJoints(Joints _joints)
//        {
//            interestedJoints = _joints;
//        }
    }
}
