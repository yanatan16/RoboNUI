using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * Servo controller command to query the pulse width of channels on the servo controller
     * 
     * This command group has return values from the servo.
     * 
     * Base class: <see cref="ServoCommandGroup"/>
     * </summary>
     * 
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     */
    class QueryPulseWidth : ServoCommandGroup
    {
        /**
         * <summary>List of channels to query pulse width</summary>
         */
        private List<uint> Channel;

        /**
         * <summary>
         * Constructor which instantiates a Pulse Width Query Servo Command Type.
         * </summary>
         */
        public QueryPulseWidth() :
            base(ServoCommandType.QueryPulseWidth)
        {
            Channel = new List<uint>();
        }

        /**
         * <summary>
         * Add channel to query command
         * </summary>
         * 
         * <param name="ch">Channel number</param>
         * <remarks>
         * Increases the response length by one
         * </remarks>
         */
        public void addChannel(uint ch)
        {
            Channel.Add(ch);
            NumCommands++;
            ResponseLength++;
        }

        /**
         * <summary>See <see cref="ServoCommandGroup.IncCommandString"/> for inherited method summary.</summary>
         */
        protected override string IncCommandString(int i)
        {
            return string.Format("QP{0}", Channel[i]);
        }

        /**
         * <summary>See <see cref="ServoCommandGroup.PostCommandString"/> for inherited method summary.</summary>
         */
        protected override string PostCommandString()
        {
            return string.Empty;
        }

        /**
         * <summary>
         * Static method to interpret the response from the servo controller
         * </summary>
         * 
         * <param name="response">Response received from servo controller</param>
         * <returns>Array of pulse widths corresponding to channels selected.</returns>
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
