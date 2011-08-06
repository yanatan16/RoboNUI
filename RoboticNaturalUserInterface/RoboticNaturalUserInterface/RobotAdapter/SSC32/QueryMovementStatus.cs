using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    /**
     * Query Movement Status
     * 
     * Base class: Servo Command Group
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * Query the movement status of the servo controller
     */
    class QueryMovementStatus : ServoCommandGroup
    {
        /**
         * Constructor
         * 
         * Construct base class with a single return byte
         */
        public QueryMovementStatus() :
            base(ServoCommandType.QueryMovementStatus, 1)
        {
        }

        /**
         * (See ServoCommandGroup.IncCommandString(int i) for comments)
         */
        protected String ServoCommandGroup.IncCommandString(int i)
        {
            return "";
        }

        /**
         * (See ServoCommandGroup.PostCommandString() for comments)
         */
        protected String ServoCommandGroup.PostCommandString()
        {
            return "Q";
        }

        /**
         * Interpret the response from the servo controller
         * Static method
         * 
         * Paramter: Response received from servo controller
         * Returns: bool true if movement in complete, false otherwise
         */
        public static bool interpretMovementStatus(byte[] response)
        {
            if (((char) response[0]) == '.')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
