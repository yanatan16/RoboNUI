using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * Query the movement status of the servo controller.
     * 
     * This command group has one return byte.
     * 
     * Base class: <see cref="ServoCommandGroup"/>
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     */
    class QueryMovementStatus : ServoCommandGroup
    {
        /**
         * <summary>Constructor to construct a Query Movement Status Command Group</summary>
         */
        public QueryMovementStatus() :
            base(ServoCommandType.QueryMovementStatus, 1)
        {
        }

        /**
         * <summary>See <see cref="ServoCommandGroup.IncCommandString"/> for inherited method summary</summary>
         */
        protected override string IncCommandString(int i)
        {
            return string.Empty;
        }

        /**
         * <summary>See <see cref="ServoCommandGroup.PostCommandString"/> for inherited method summary</summary>
         */
        protected override string PostCommandString()
        {
            return "Q";
        }

        /**
         * <summary>
         * Static method to interpret the response from the servo controller
         * </summary>
         * <param name="response">Response received from servo controller</param>
         * <returns>Boolean true if movement in complete, false otherwise</returns>
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
