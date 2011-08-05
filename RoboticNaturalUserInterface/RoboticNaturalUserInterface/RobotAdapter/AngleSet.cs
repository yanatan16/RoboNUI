using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.RobotAdapter
{
    struct AngleSet
    {
        public Dictionary<RoboticAngle, ulong> AngleMap;

        public void setMap(RoboticAngle[] keyArray, ulong[] valArray)
        {
            for (int i = 0; i < keyArray.Length; i++)
            {
                AngleMap[keyArray[i]] = valArray[i];
            }
        }
    }
}
