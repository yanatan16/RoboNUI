using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI
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
        public void updateAngles(AngleSet angles);
    }
}
