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
     * This class controls the Arm servos based on angles
     * passed in through the IRoboticAngleConsumer interface.
     * 
     * Interface: IRoboticAngleConsumer
     */
    class RoboticArmServoController : ServoController, IRoboticAngleConsumer
    {
        private Dictionary<RoboticAngle, uint> channelMap_;
        private ulong speed_;

        public RoboticArmServoController(string portName, Dictionary<RoboticAngle, uint> channelMap, ulong speed = 0) :
            base(portName)
        {
            channelMap_ = channelMap;
            speed_ = speed;
        }

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
