using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    class ServoMovementCommand : ServoCommandGroup
    {
        private List<uint> channel; //Channel number (0-31)
        private List<ulong> pulseWidth; //Pulse Width in usec
        private List<ulong> moveSpeed; //Speed of incremental move (usec/sec)
        private ulong totalTime; //Time for entire move (msec)

        public ServoMovementCommand() :
            base(ServoCommandGroup.ServoCommandType.ServoMovement)
        {
            channel = new List<uint>();
            pulseWidth = new List<ulong>();
            moveSpeed = new List<ulong>();
            totalTime = 0;
        }

        /**
         * Add a servo movement command
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

        protected string ServoCommandGroup.IncCommandString(int i)
        {
            string ret = string.Empty;
            //Required parameters
            ret += string.Format("#%ud P%uld", channel[i], pulseWidth[i]);
            if (moveSpeed[i] > 0)
                ret += string.Format(" S%uld", moveSpeed[i]);
            return ret;
        }

        protected string ServoCommandGroup.PostCommandString()
        {
            if (totalTime > 0)
                return string.Format("T%uld", totalTime);
            else
                return string.Empty;
        }
    }
}
