using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using RoboNUI.RobotAdapter.SSC32;
using RoboNUI.Core;

namespace RoboNUI.RobotAdapter
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
        private Dictionary<RoboticAngle, uint> channelMap_;

        /**
         * Generic speed for each movement
         */
        private ulong speed_;

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
            channelMap_ = channelMap;
            speed_ = speed;
        }

        /**
         * (See IRoboticAngleConsumer.updateAngles(AngleSet angles) for comments)
         */
        public void IRoboticAngleConsumer.updateAngles(AngleSet angles)
        {
            ServoMovementCommand command = new ServoMovementCommand();
            for (Dictionary<RoboticAngle, ulong>.Enumerator en = angles.AngleMap.GetEnumerator(); en.MoveNext(); )
            {
                command.addServoMovementCommand(channelMap_[en.Current.Key], en.Current.Value, speed_);
            }

            sendCommand(command);
        }

        /**
         * Get positions of joints
         * 
         * Parameter: Joint list - list of joints requesting positions on
         * Returns: Angle set of joint positions
         */
        public AngleSet getPositions(List<RoboticAngle> jointList)
        {
            QueryPulseWidth command = new QueryPulseWidth();
            for (List<RoboticAngle>.Enumerator en = jointList.GetEnumerator(); en.MoveNext(); )
            {
                command.addChannel(channelMap_[en.Current]);
            }

            byte[] response = sendCommand(command);
            ulong[] angles = QueryPulseWidth.interpretPulseWidths(response);
            
            AngleSet ret = new AngleSet();
            ret.setMap(jointList.ToArray(), angles);

            return ret;
        }

        /**
         * Query if movement is finished
         * 
         * Returns: bool true if movement is finished, false otherwise
         */
        public bool isMovementFinished()
        {
            QueryMovementStatus command = new QueryMovementStatus();
            byte[] response = sendCommand(command);
            return QueryMovementStatus.interpretMovementStatus(response);
        }
    }
}
