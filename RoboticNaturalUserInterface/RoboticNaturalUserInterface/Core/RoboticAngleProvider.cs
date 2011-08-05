using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.Core
{
    /**
     * Roboitc Angle Provider Base Class
     * 
     * This base class provides tooling for the list of robotic angle consumers
     * always used by an robotic angle provider. Base off of this class to be used as 
     * an angle provider by the State Manager and ease the use of sending to multiple
     * angle consumers.
     */
    class RoboticAngleProvider
    {
        private List<IRoboticAngleConsumer> angleConsumers;

        /**
         * Add an angle consumer to the list
         */
        public void addAngleConsumer(IRoboticAngleConsumer newAngleConsumer)
        {
            angleConsumers.Add(newAngleConsumer);
        }

        /**
         * Remove an angle consumer from the list
         */
        public void removeAngleConsumer(IRoboticAngleConsumer oldAngleConsumer)
        {
            angleConsumers.Remove(oldAngleConsumer);
        }

        /**
         * Send angles to all consumers
         * 
         * Parameters: angles - the set of angles to send
         */
        protected void sendAngles(AngleSet angles)
        {
            for (List<IRoboticAngleConsumer>.Enumerator en = angleConsumers.GetEnumerator(); en.MoveNext(); )
            {
                (en.Current as IRoboticAngleConsumer).updateAngles(angles);
            }
        }
    }
}
