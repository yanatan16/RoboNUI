using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
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
        private List<uint> channel;

        /**
         * Pulse width to command (position)
         * In unites of usec
         * Range: 0 - 3000, 1500 is middle
         */
        private List<ulong> pulseWidth;

        /**
         * Movement speed to command, optional
         * In units of usec / sec
         * Only limits speed, may go slower, may go slower if time denotes
         */
        private List<ulong> moveSpeed; 

        /**
         * Total time for entire movement command group
         * In unites of msec
         * Only limits speed, may go slower if speed denotes
         */
        private ulong totalTime;

        /**
         * Constructor
         * 
         * Constructs base class, and instantiates class variables
         */
        public ServoMovementCommand() :
            base(ServoCommandGroup.ServoCommandType.ServoMovement)
        {
            channel = new List<uint>();
            pulseWidth = new List<ulong>();
            moveSpeed = new List<ulong>();
            totalTime = 0;
        }

        /**
         * Add a servo movement command to the command group
         * 
         * Pameters: Channel number, pulse width and movement speed (optional)
         */
        public void addServoMovementCommand(uint ch, ulong pw, ulong ms = 0)
        {
            base.incrementNumCommands();
            channel.Add(ch);
            pulseWidth.Add(pw);
            moveSpeed.Add(ms);
        }
        
        /**
         * Set the total time for movement
         * 
         * Parameter: total time in ms
         */
        public void setTotalTime(ulong tt)
        {
            totalTime = tt;
        }

        /**
         * (See ServoCommandGroup.IncCommandString(int i) for comments)
         */
        protected string ServoCommandGroup.IncCommandString(int i)
        {
            string ret = string.Empty;
            //Required parameters
            ret += string.Format("#%ud P%uld", channel[i], pulseWidth[i]);
            if (moveSpeed[i] > 0)
                ret += string.Format(" S%uld", moveSpeed[i]);
            return ret;
        }

        /**
         * (See ServoCommandGroup.PostCommandString() for comments)
         */
        protected string ServoCommandGroup.PostCommandString()
        {
            if (totalTime > 0)
                return string.Format("T%uld", totalTime);
            else
                return string.Empty;
        }
    }
}
