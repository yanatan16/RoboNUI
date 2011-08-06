using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RobotAdapter
{
    /**
     * Robotic Servo Controller - Marionette
     * 
     * This class controls the Marionette servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     * 
     * Interface: IRoboticAngleConsumer
     */
    class RoboticMarionetteServoController : IRoboticAngleConsumer
    {


        void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
