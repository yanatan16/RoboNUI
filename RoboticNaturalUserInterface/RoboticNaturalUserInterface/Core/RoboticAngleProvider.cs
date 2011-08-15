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
     * </remarks> Author: Jon Eisen (yanatan16@gmail.com) </remarks>
     * <seealso>IRoboticAngleConsumer</seealso>
     */
    class RoboticAngleProvider
    {
        /**
         * <summary>
         * List of <see cref="IRoboticAngleConsumer"/>angle consumers</see> for this angle provider
         * </summary>
         */
        private List<IRoboticAngleConsumer> angleConsumers;

        /**
         * <summary>Add an angle consumer to the list</summary>
         * <param name="newAngleConsumer">An angle consumer to add to the list</param>
         */
        public void addAngleConsumer(IRoboticAngleConsumer newAngleConsumer)
        {
            angleConsumers.Add(newAngleConsumer);
        }

        /**
         * <summary>Remove an angle consumer from the list</summary>
         * <param name="oldAngleConsumer">An angle consumer to remove from the list</param>
         */
        public void removeAngleConsumer(IRoboticAngleConsumer oldAngleConsumer)
        {
            angleConsumers.Remove(oldAngleConsumer);
        }

        /**
         * <summary>
         * Send angles to all consumers
         * </summary>
         * <param name="angles">The set of angles to send</param>
         */
        protected void SendAngles(AngleSet angles)
        {
            for (List<IRoboticAngleConsumer>.Enumerator en = angleConsumers.GetEnumerator(); en.MoveNext(); )
            {
                (en.Current as IRoboticAngleConsumer).UpdateAngles(angles);
            }
        }
    }
}
