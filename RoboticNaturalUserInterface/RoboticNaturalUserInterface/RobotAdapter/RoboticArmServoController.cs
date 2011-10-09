using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using RoboNui.RobotAdapter.SSC32;
using RoboNui.Core;
using Utilities.Messaging;

using log4net;

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class controls the Arm servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     * 
     * Base class: <see cref="ServoController"/>
     * Interface: <see cref="T:IConsumer{AngleSet}"/>
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * <seealso cref="ServoController"/>
     * <seealso cref="T:IConsumer{AngleSet}"/>
     */
    class RoboticArmServoController : ServoController, IConsumer<AngleSet>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        /**
         * <summary>
         * Mapping of RoboNUI.RoboticAngle to SSC-32 servo channels
         * </summary>
         */
        public Dictionary<RoboticAngle, uint> ChannelMap { get; set; }

        /**
         * <summary>
         * Generic speed for each movement
         * </summary>
         * <remarks>In units of microsecond per second</remarks>
         */
        public ulong Speed { get; set; }

        /**
         * <summary>
         * A set of pulse width constants for this consumer to use
         * </summary>
         */
        protected PulseWidthConstants PulseWidthConverter;

        /**
         * <summary>
         * Constructor with port name, channel map, and default movement speed
         * </summary>
         * <param name="channelMap">Map of robotic angles to channel number</param>
         * <param name="portName">The serial port of the servo controller</param>
         * <param name="speed">Optional generic speed of each movement</param>
         */
        public RoboticArmServoController(string portName, Dictionary<RoboticAngle, uint> channelMap, ulong speed = 0) :
            base(portName)
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            PulseWidthConverter = new PulseWidthConstants(1500 / Math.PI, 1500);
            ChannelMap = channelMap;
            Speed = speed;
        }

        /**
         * <summary>See <see cref="M:IConsumer{AngleSet}.Update"/> for inherited method summary.</summary>
         */
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            ServoMovementCommand command = new ServoMovementCommand();
            foreach (KeyValuePair<RoboticAngle, ulong> angle in angles.GetPulseWidthMap(PulseWidthConverter))
            {
                command.addServoMovementCommand(ChannelMap[angle.Key], angle.Value, 0);
            }
            command.TotalTime = Speed;
            log.Info("Sent movement command to Servo Controller.");

            sendCommand(command);
        }

        /**
         * <summary>Get positions of joints</summary>
         * 
         * <param name="roboticAngleList">List of joints to retreive angle positions for</param>
         * Joint list - list of joints requesting positions on
         * <returns>Angle set of joint positions</returns>
         */
        public AngleSet GetAngles(List<RoboticAngle> roboticAngleList)
        {
            QueryPulseWidth command = new QueryPulseWidth();
            foreach (RoboticAngle ra in roboticAngleList)
            {
                command.addChannel(ChannelMap[ra]);
            }

            byte[] response = sendCommand(command);
            
            if (response == null)
            {
                return null;
            }

            ulong[] pws = QueryPulseWidth.interpretPulseWidths(response);

            Dictionary<RoboticAngle, ulong> pwMap = new Dictionary<RoboticAngle, ulong>();
            for (int i = 0; i < response.Length; i++)
            {
                pwMap[roboticAngleList[i]] = pws[i];
            }
            
            AngleSet ret = new AngleSet();
            ret.SetPulseWidthMap(pwMap, PulseWidthConverter);

            return ret;
        }

        /**
         * <summary>Query if movement is finished</summary>
         * 
         * <returns>Boolean true if movement is finished, false otherwise</returns>
         */
        public bool IsMovementFinished()
        {
            QueryMovementStatus command = new QueryMovementStatus();
            byte[] response = sendCommand(command);
            return QueryMovementStatus.interpretMovementStatus(response);
        }
    }
}
