using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI
{
    /**
     * Recorder
     * 
     * This class records angles sent to the Robotic servo controllers
     * and stores them for later.
     * 
     * Interface: IRoboticAngleConsumer
     */
    class Recorder : IRoboticAngleConsumer
    {
        void IRoboticAngleConsumer.updateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
