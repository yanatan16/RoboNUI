using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RecordPlayback
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
        void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
