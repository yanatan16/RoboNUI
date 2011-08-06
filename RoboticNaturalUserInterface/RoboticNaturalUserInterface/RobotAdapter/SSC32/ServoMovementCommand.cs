using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * Servo Movement Command
     * 
     * Base class: Servo Command Group
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * A representation for a servo movement command to an SSC-32 servo controller
     */
    class ServoMovementCommand : ServoCommandGroup
    {
        /**
         * Channel number to command (motor)
         * Range: 0 - 31
         */
        private List<uint> Channel;

        /**
         * Pulse width to command (position)
         * In unites of usec
         * Range: 0 - 3000, 1500 is middle
         */
        private List<ulong> PulseWidth;

        /**
         * Movement speed to command, optional
         * In units of usec / sec
         * Only limits speed, may go slower, may go slower if time denotes
         */
        private List<ulong> MoveSpeed; 

        /**
         * Total time for entire movement command group
         * In unites of msec
         * Only limits speed, may go slower if speed denotes
         */
        public ulong TotalTime { get; set; }

        /**
         * Constructor
         * 
         * Constructs base class, and instantiates class variables
         */
        public ServoMovementCommand() :
            base(ServoCommandGroup.ServoCommandType.ServoMovement)
        {
            Channel = new List<uint>();
            PulseWidth = new List<ulong>();
            MoveSpeed = new List<ulong>();
            TotalTime = 0;
        }

        /**
         * Add a servo movement command to the command group
         * 
         * Pameters: Channel number, pulse width and movement speed (optional)
         */
        public void addServoMovementCommand(uint ch, ulong pw, ulong ms = 0)
        {
            NumCommands++;
            Channel.Add(ch);
            PulseWidth.Add(pw);
            MoveSpeed.Add(ms);
        }
        
        /**
         * (See ServoCommandGroup.IncCommandString(int i) for comments)
         */
        protected string ServoCommandGroup.IncCommandString(int i)
        {
            string ret = string.Empty;
            //Required parameters
            ret += string.Format("#%ud P%uld", Channel[i], PulseWidth[i]);
            if (MoveSpeed[i] > 0)
                ret += string.Format(" S%uld", MoveSpeed[i]);
            return ret;
        }

        /**
         * (See ServoCommandGroup.PostCommandString() for comments)
         */
        protected string ServoCommandGroup.PostCommandString()
        {
            if (TotalTime > 0)
                return string.Format("T%uld", TotalTime);
            else
                return string.Empty;
        }
    }
}
