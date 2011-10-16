using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;
using Utilities.Messaging;

using log4net;
using RoboNui.RobotAdapter.MiniSSC2;

// TODO Implement

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class controls the Marionette servos based on angles passed in through the <see cref="T:IConsumer"/> interface.
     * 
     * Interface: <see cref="T:IConsumer"/> with T = <see cref="AngleSet"/>
     * </summary>
     */
    class RoboticMarionetteServoController : MiniSSCIIServoController, IConsumer<AngleSet>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private readonly ILog log = LogManager.GetLogger(typeof(RoboticMarionetteServoController));

        /**
         * <summary>
         * Mapping of RoboNUI.RoboticAngle to Mini SSC II servo channels
         * </summary>
         */
        public Dictionary<RoboticAngle, uint> ChannelMap { get; set; }

        /**
         * <summary>
         * A set of pulse width constants for this consumer to use
         * </summary>
         */
        protected PulseWidthConstants PulseWidthConverter;

        public RoboticMarionetteServoController(string portName, Dictionary<RoboticAngle, uint> channelMap) :
            base(portName, channelMap.Values)
        {
            log.Debug(this.ToString() + " constructed.");

            PulseWidthConverter = new PulseWidthConstants(128 / Math.PI, 128);
            ChannelMap = channelMap;

            ServoMovementCommand smc = new ServoMovementCommand(channelMap[RoboticAngle.CurtainOpen], 0);
            sendCommand(smc);
        }

        /**
         * <summary>See <see cref="M:IConsumer.Update"/> for the inherited method summary</summary>
         */
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            foreach (KeyValuePair<RoboticAngle, ulong> pair in angles.GetPulseWidthMap(PulseWidthConverter))
            {
                byte pw = (byte) Math.Min(255, pair.Value);
                ServoMovementCommand smc = new ServoMovementCommand(ChannelMap[pair.Key], pw);
                sendCommand(smc);
            }
            log.Info("Sent " + angles.AngleMap.Count + " movement commands to the Servo Controller.");
        }
    }
}
