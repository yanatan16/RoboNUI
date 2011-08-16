using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;
using RoboNui.Messaging;

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class controls the Marionette servos based on angles passed in through the <see cref="T:IConsumer"/> interface.
     * 
     * Interface: <see cref="T:IConsumer"/> with T = <see cref="AngleSet"/>
     * </summary>
     */
    class RoboticMarionetteServoController : IConsumer<AngleSet>
    {

        /**
         * <summary>See <see cref="M:IConsumer.Update"/> for the inherited method summary</summary>
         */
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
