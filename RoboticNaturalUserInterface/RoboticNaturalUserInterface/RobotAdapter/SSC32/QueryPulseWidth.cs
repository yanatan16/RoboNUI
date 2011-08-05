using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    class QueryPulseWidth : ServoCommandGroup
    {
        private List<uint> channel;

        public QueryPulseWidth() :
            base(ServoCommandType.QueryPulseWidth)
        {
            channel = new List<uint>();
        }

        public void addChannel(uint ch)
        {
            channel.Add(ch);
            base.incrementNumCommands();
            base.incrementResponseLength();
        }

        protected string ServoCommandGroup.IncCommandString(int i)
        {
            if (i == 0)
                return String.Format("QP %ud", channel[i]);
            else
                return String.Format("%ud", channel[i]);
        }

        protected string ServoCommandGroup.PostCommandString()
        {
            return "";
        }

        /**
         * Interpret the response from the servo controller
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
