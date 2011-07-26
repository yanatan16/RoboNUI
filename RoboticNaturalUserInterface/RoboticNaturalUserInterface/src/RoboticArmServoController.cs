using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI
{
    /**
     * Robotic Servo Controller - Arm
     * 
     * This class controls the Arm servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     * 
     * Interface: IRoboticAngleConsumer
     */
    class RoboticArmServoController : IRoboticAngleConsumer
    {
        void IRoboticAngleConsumer.updateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
