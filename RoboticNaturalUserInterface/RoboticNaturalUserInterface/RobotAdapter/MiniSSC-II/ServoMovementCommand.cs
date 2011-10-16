using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//TODO Implement

namespace RoboNui.RobotAdapter.MiniSSC2
{
    /**
     * <summary>
     * A servo command for servo movement to an Mini-SSC II servo controller
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     */
    class ServoMovementCommand
    {
        /**
         * <summary>Channel number to command (motor)</summary>
         * <remarks>Range: 0 - 31</remarks>
         */
        private uint Channel;

        /**
         * <summary>Pulse width to command (position)</summary>
         * <remarks>
         * In units of microseconds.
         * Range: 0 - 3000, 1500 is middle
         * </remarks>
         */
        private byte PulseWidth;

        /**
         * <summary>
         * Constructor for this class.
         * Instantiates a ServoMovement command from base class.
         * </summary>
         */
        public ServoMovementCommand()
        {
            Channel = 0;
            PulseWidth = 0;
        }

        /**
         * <summary>
         * Constructor for this class with parameters.
         * Instantiates a ServoMovement command from base class.
         * </summary>
         */
        public ServoMovementCommand(uint ch, byte pw)
        {
            Channel = ch;
            PulseWidth = pw;
        }

        /**
        * <summary>Construct the command string to send to the servo controller</summary>
        * 
        * <returns>The command string to send</returns>
        * <seealso cref="MiniSSCIIServoController"/>
        */
        public IEnumerable<byte> CommandString()
        {
            List<byte> ret = new List<byte>();
            ret.Add(255);
            ret.Add((byte)Channel);
            ret.Add(PulseWidth);
            return ret;
        }

    }

}
