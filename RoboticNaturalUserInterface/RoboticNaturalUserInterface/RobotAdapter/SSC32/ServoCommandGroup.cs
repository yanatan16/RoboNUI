using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    abstract class ServoCommandGroup
    {
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

        private ServoCommandType type_;
        private uint numCommands_;
        private uint responseLength_;

        protected ServoCommandGroup(ServoCommandType type, uint responseLength = 0, uint numCommands = 0)
        {
            type_ = type;
            responseLength_ = responseLength;
            numCommands_ = numCommands;
        }

        protected uint getNumCommands()
        {
            return numCommands_;
        }

        protected void setNumCommands(uint numCommands)
        {
            numCommands_ = numCommands;
        }

        protected void incrementNumCommands()
        {
            numCommands_++;
        }

        public bool hasResponse()
        {
            return responseLength_ > 0;
        }

        public uint getResponseLength()
        {
            return responseLength_;
        }

        public void setResponseLength(uint rl)
        {
            responseLength_ = rl;
        }

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
        * 
        * Parameter: Index of command to output
        * Returns: Command string for command index as parameter
        */
        protected abstract string IncCommandString(int commandIndex);

        /**
        * Post-Command String
        * 
        * Returns: String of command after all incremental commands
        */
        protected abstract string PostCommandString();
    }
}
