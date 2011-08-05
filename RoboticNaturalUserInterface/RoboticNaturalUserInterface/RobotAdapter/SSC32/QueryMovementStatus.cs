using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter.SSC32
{
    class QueryMovementStatus : ServoCommandGroup
    {
        public QueryMovementStatus() :
            base(ServoCommandType.QueryMovementStatus, 1)
        {
        }

        protected String ServoCommandGroup.IncCommandString(int i)
        {
            return "";
        }

        protected String ServoCommandGroup.PostCommandString()
        {
            return "Q";
        }

        /**
         * Interpret the response from the servo controller
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
