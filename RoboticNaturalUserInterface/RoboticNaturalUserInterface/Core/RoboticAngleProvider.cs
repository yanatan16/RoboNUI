using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * <summary>
     * This base class provides tooling for the list of robotic angle consumers
     * always used by an robotic angle provider. Base off of this class to be used as 
     * an angle provider by the State Manager and ease the use of sending to multiple
     * angle consumers.
     * </summary>
     * 
     * <remarks> Author: Jon Eisen (yanatan16@gmail.com) </remarks>
     * <seealso cref="T:RoboNui.Core.IRoboticAngleConsumer"/>
     */
    class RoboticAngleProvider
    {
        /**
         * <summary>
         * List of <see cref="IRoboticAngleConsumer"/> registered to this angle provider
         * </summary>
         */
        private List<IRoboticAngleConsumer> AngleConsumers;

        /**
         * <summary>
         * Constructor for this class
         * </summary>
         */
        protected RoboticAngleProvider()
        {
            AngleConsumers = new List<IRoboticAngleConsumer>();
        }

        /**
         * <summary>Add an angle consumer to the list</summary>
         * <param name="newAngleConsumer">An angle consumer to add to the list</param>
         */
        public void addAngleConsumer(IRoboticAngleConsumer newAngleConsumer)
        {
            AngleConsumers.Add(newAngleConsumer);
        }

        /**
         * <summary>Remove an angle consumer from the list</summary>
         * <param name="oldAngleConsumer">An angle consumer to remove from the list</param>
         */
        public void removeAngleConsumer(IRoboticAngleConsumer oldAngleConsumer)
        {
            AngleConsumers.Remove(oldAngleConsumer);
        }

        /**
         * <summary>
         * Send angles to all consumers
         * </summary>
         * <param name="angles">The set of angles to send</param>
         */
        protected void SendAngles(AngleSet angles)
        {
            for (List<IRoboticAngleConsumer>.Enumerator en = AngleConsumers.GetEnumerator(); en.MoveNext(); )
            {
                (en.Current as IRoboticAngleConsumer).UpdateAngles(angles);
            }
        }
    }
}
