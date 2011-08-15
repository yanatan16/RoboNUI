using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * An abstract base class representation of a servo command group to be sent to an SSC-32 servo controller.
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * <seealso cref="QueryMovementStatus"/>
     * <seealso cref="QueryPulseWidth"/>
     * <seealso cref="ServoMovementCommand"/>
     * <seealso cref="ServoController"/>
     */
    abstract class ServoCommandGroup
    {
        /**
         * <summary
         * An enumeration of the 12 command group types that can be sent to an SSC-32.
         * </summary>
         */
        public enum ServoCommandType
        {
            ServoMovement = 1,
            DiscreteOutput = 2,
            ByteOutput = 3,
            QueryMovementStatus = 4,
            QueryPulseWidth = 5,
            ReadDigitalInputs = 6,
            ReadAnalogInputs = 7,
            TwelveServoHexapodGaitSequencer = 8,
            QueryHexSequencer = 9,
            GetVersion = 10,
            GoToBoot = 11,
            MiniSSC2Compatibility = 12
        }

        /**
         * <summary>Type of Servo Command</summary>
         */
        public ServoCommandType ComType { get; private set; }

        /**
         * <summary>Number of commands in this command group</summary>
         */
        protected uint NumCommands { get; set; }

        /**
         * <summary>Length of the response</summary>
         * <remarks>In units of bytes</remarks>
         */
        public uint ResponseLength;

        /**
         * <summary>
         * Constructor for a servo command group
         * </summary>
         * 
         * <param name="numCommands">Number of commands</param>
         */
        protected ServoCommandGroup(ServoCommandType type, uint responseLength = 0, uint numCommands = 0) 
        {
            ComType = type;
            ResponseLength = responseLength;
            NumCommands = numCommands;
        }

        /**
        * <summary>Construct the command string to send to the servo controller</summary>
        * 
        * <returns>The command string to send</summary>
        * <seealso cref="ServoController"/>
        */
        public string CommandString()
        {
            string ret = string.Empty;
            for (int i = 0; i < NumCommands; i++)
            {
                ret += IncCommandString(i) + " ";
            }
            ret += PostCommandString() + " " + ((char)13);
            return ret;
        }

        /**
        * <summary>
        * Abstract method to get the command string for each individual command sequentially
        * </summary>
        * 
        * <param name="commandIndex">Index of command to output</param>
        * <returns>Incremental command string to send to the servo controller</returns>
        */
        protected abstract string IncCommandString(int commandIndex);

        /**
        * <summary>
        * Abstract method to get the end of the command string
        * </summary>
        * 
        * <returns>End of the command string to send to the servo controller</returns>
        */
        protected abstract string PostCommandString();
    }
}
