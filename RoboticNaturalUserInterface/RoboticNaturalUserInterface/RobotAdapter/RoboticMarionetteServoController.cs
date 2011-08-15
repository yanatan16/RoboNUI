using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class controls the Marionette servos based on angles passed in through the <see cref="IRoboticAngleConsumer"/> interface.
     * 
     * Interface: <see cref="IRoboticAngleConsumer"/>
     * </summary>
     */
    class RoboticMarionetteServoController : IRoboticAngleConsumer
    {

        /**
         * <summary>See <see cref="IRoboticAngleConsumer.UpdateAngles"/> for inherited method summary.</summary>
         */
        void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
