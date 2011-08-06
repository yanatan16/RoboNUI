using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    /**
     * Servo Command Group
     * 
     * Abstract Base Class
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * A representation of a servo command group to be sent to an SSC-32
     * servo controller
     */
    abstract class ServoCommandGroup
    {
        /**
         * Servo Command Type
         * 
         * An enumeration of the 12 commands that can be sent to an SSC-32
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
         * Type of Servo Command
         */
        private ServoCommandType type_;

        /**
         * Number of commands in this command group
         */
        private uint numCommands_;

        /**
         * Length of the response in bytes
         */
        private uint responseLength_;

        /**
         * Constructor
         * 
         * Parameters: Type, response length, and number of commands
         */
        protected ServoCommandGroup(ServoCommandType type, uint responseLength = 0, uint numCommands = 0)
        {
            type_ = type;
            responseLength_ = responseLength;
            numCommands_ = numCommands;
        }

        /**
         * Getter for numCommands
         */
        protected uint getNumCommands()
        {
            return numCommands_;
        }

        /**
         * Setter for numCommands
         */
        protected void setNumCommands(uint numCommands)
        {
            numCommands_ = numCommands;
        }

        /**
         * Incrementer for numCommands
         */
        protected void incrementNumCommands()
        {
            numCommands_++;
        }


        /**
         * Has Response
         * 
         * Returns: boolean true if response is expected
         */
        public bool hasResponse()
        {
            return responseLength_ > 0;
        }

        /**
         * Getter for responseLength
         */
        public uint getResponseLength()
        {
            return responseLength_;
        }

        /**
         * Setter for responseLength
         */
        public void setResponseLength(uint rl)
        {
            responseLength_ = rl;
        }

        /**
         * Incrementer for responseLength
         */
        public void incrementResponseLength()
        {
            responseLength_++;
        }

        /**
        * Command String
        * 
        * Returns: The command string to send to the Servo Controller
        */
        public string CommandString()
        {
            string ret = string.Empty;
            for (int i = 0; i < numCommands_; i++)
            {
                ret += IncCommandString(i) + " ";
            }
            ret += PostCommandString() + " " + ((char)13);
            return ret;
        }

        /**
        * Incremental Command String
        * Abstract method
        * 
        * Get the command string for each individual command
        * 
        * Parameter: Index of command to output
        * Returns: Command string for command index as parameter
        */
        protected abstract string IncCommandString(int commandIndex);

        /**
        * Post-Command String
        * Abstract method
        * 
        * Get the command string after all individual commands
        * 
        * Returns: String of command after all incremental commands
        */
        protected abstract string PostCommandString();
    }
}
