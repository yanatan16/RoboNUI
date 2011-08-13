using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using RoboNui.RobotAdapter.SSC32;
using RoboNui.Core;

namespace RoboNui.RobotAdapter
{
    /**
     * Robotic Servo Controller - Arm
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * Base class: ServoController
     * Interface: IRoboticAngleConsumer
     * 
     * This class controls the Arm servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     */
    class RoboticArmServoController : ServoController, IRoboticAngleConsumer
    {
        /**
         * Mapping of RoboNUI.RoboticAngle to SSC-32 servo channels
         */
        public Dictionary<RoboticAngle, uint> ChannelMap { get; set; }

        /**
         * Generic speed for each movement
         */
        public ulong Speed { get; set; }

        /**
         * A set of pulse width constants for this consumer to use
         */
        protected PulseWidthConstants MyPulseWidthConstants;

        /**
         * Constructor
         * 
         * Construct class with port name, channel map, and default movement speed
         * 
         * Parameters: port name, channel map, and speed (usec/sec)
         */
        public RoboticArmServoController(string portName, Dictionary<RoboticAngle, uint> channelMap, ulong speed = 0) :
            base(portName)
        {
            MyPulseWidthConstants = new PulseWidthConstants(1500 / Math.PI, 1500);
            ChannelMap = channelMap;
            Speed = speed;
        }

        /**
         * (See IRoboticAngleConsumer.updateAngles(AngleSet angles) for comments)
         */

        public void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            ServoMovementCommand command = new ServoMovementCommand();
            for (Dictionary<RoboticAngle, ulong>.Enumerator en = angles.getPulseWidthMap(MyPulseWidthConstants).GetEnumerator(); en.MoveNext(); )
            {
                command.addServoMovementCommand(ChannelMap[en.Current.Key], en.Current.Value, Speed);
            }

            sendCommand(command);
        }

        /**
         * Get positions of joints
         * 
         * Parameter: Joint list - list of joints requesting positions on
         * Returns: Angle set of joint positions
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
         * Query if movement is finished
         * 
         * Returns: bool true if movement is finished, false otherwise
         */
        public bool IsMovementFinished()
        {
            QueryMovementStatus command = new QueryMovementStatus();
            byte[] response = sendCommand(command);
            return QueryMovementStatus.interpretMovementStatus(response);
        }
    }
}
