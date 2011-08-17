using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;
using Utilities.Messaging;

using log4net;

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
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        public RoboticMarionetteServoController()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");
        }

        /**
         * <summary>See <see cref="M:IConsumer.Update"/> for the inherited method summary</summary>
         */
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
