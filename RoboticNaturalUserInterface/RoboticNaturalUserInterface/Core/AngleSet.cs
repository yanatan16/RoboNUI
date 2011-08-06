using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.Core
{
    /**
     * Angle Set
     * 
     * A single struct that contains the angles of the robot
     */
    struct AngleSet
    {
        /**
         * Mapping of RoboNUI Robotic Angles to angle values
         * Angle values stored as ulong, range 0 - 3000, with 1500 in the center
         */
        public Dictionary<RoboticAngle, ulong> AngleMap;

        /**
         * Set the map with a list of keys and values
         * 
         * Parameters: key array and val array (must be same size)
         */
        public void setMap(RoboticAngle[] keyArray, ulong[] valArray)
        {
            for (int i = 0; i < keyArray.Length; i++)
            {
                AngleMap[keyArray[i]] = valArray[i];
            }
        }
    }
}
