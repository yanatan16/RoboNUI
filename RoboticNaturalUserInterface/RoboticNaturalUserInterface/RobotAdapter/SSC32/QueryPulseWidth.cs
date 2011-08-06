using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * Query Pulse Width Servo Command
     * 
     * Base class: Servo Command Group
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * Query the pulse width of a list of channels
     */
    class QueryPulseWidth : ServoCommandGroup
    {
        /**
         * List of channels to query pulse width
         */
        private List<uint> Channel;

        /**
         * Constructor
         * 
         * Construct base class and instantiate class variable
         */
        public QueryPulseWidth() :
            base(ServoCommandType.QueryPulseWidth)
        {
            Channel = new List<uint>();
        }

        /**
         * Add channel to query command
         * 
         * Parameter: channel number
         */
        public void addChannel(uint ch)
        {
            Channel.Add(ch);
            NumCommands++;
            ResponseLength++;
        }

        /**
         * (See ServoCommandGroup.IncCommandString(int i) for comments)
         */
        protected string ServoCommandGroup.IncCommandString(int i)
        {
            if (i == 0)
                return String.Format("QP %ud", Channel[i]);
            else
                return String.Format("%ud", Channel[i]);
        }

        /**
         * (See ServoCommandGroup.PostCommandString() for comments)
         */
        protected string ServoCommandGroup.PostCommandString()
        {
            return "";
        }

        /**
         * Interpret the response from the servo controller
         * Static method
         * 
         * Paramter: Response received from servo controller
         * Returns: bool true if movement in complete, false otherwise
         */
        public static ulong[] interpretPulseWidths(byte[] response)
        {
            ulong[] ret = new ulong[response.Length];
            for (int i = 0; i < response.Length; i++)
            {
                ret[i] = (ulong)response[i] * 10;
            }
            return ret;
        }
    }
}
