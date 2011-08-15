﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using RoboNui.RobotAdapter.SSC32;
using RoboNui.Core;

namespace RoboNui.RobotAdapter
{
    /**
     * <summary>
     * This class controls the Arm servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     * 
     * Base class: <see cref="ServoController"/>
     * Interface: <see cref="IRoboticAngleConsumer"/>
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * <seealso cref="ServoController"/>
     * <seealso cref="IRoboticAngleConsumer"/>
     */
    class RoboticArmServoController : ServoController, IRoboticAngleConsumer
    {
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
        protected PulseWidthConstants MyPulseWidthConstants;

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
            MyPulseWidthConstants = new PulseWidthConstants(1500 / Math.PI, 1500);
            ChannelMap = channelMap;
            Speed = speed;
        }

        /**
         * <summary>See <see cref="IRoboticAngleConsumer.UpdateAngles"/> for inherited method summary.</summary>
         */
        void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            ServoMovementCommand command = new ServoMovementCommand();
            for (Dictionary<RoboticAngle, ulong>.Enumerator en = angles.getPulseWidthMap(MyPulseWidthConstants).GetEnumerator(); en.MoveNext(); )
            {
                command.addServoMovementCommand(ChannelMap[en.Current.Key], en.Current.Value, Speed);
            }

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
            for (List<RoboticAngle>.Enumerator en = roboticAngleList.GetEnumerator(); en.MoveNext(); )
            {
                command.addChannel(ChannelMap[en.Current]);
            }

            byte[] response = sendCommand(command);
            ulong[] pws = QueryPulseWidth.interpretPulseWidths(response);

            Dictionary<RoboticAngle, ulong> pwMap = new Dictionary<RoboticAngle, ulong>();
            for (int i = 0; i < response.Length; i++)
            {
                pwMap[roboticAngleList[i]] = pws[i];
            }
            
            AngleSet ret = new AngleSet();
            ret.setPulseWidthMap(pwMap, MyPulseWidthConstants);

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
