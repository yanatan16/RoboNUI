using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.Core
{
    /**
     * Robotic Angle Consumer Interface
     * 
     * Author: Jon Eisen (jonathan.eisen@ngc.com)
     * 
     * Provide an interface for receiving updated angles from the JAT.
     */
    interface IRoboticAngleConsumer
    {

        /**
         * Update the angles to this consumer
         * 
         * Parameter: Angle set
         */
        public void UpdateAngles(AngleSet angles);
    }
}
