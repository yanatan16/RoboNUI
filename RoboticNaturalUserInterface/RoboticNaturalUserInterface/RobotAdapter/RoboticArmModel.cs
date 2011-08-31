using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RobotAdapter
{
    class RoboticArmModel : IRoboticModel
    {
        /**
         * <see cref="IRoboticModel.Translate"/>
         */
        public AngleSet Translate(JointSet js)
        {
            //TODO: Implement this translation model based on Joints to Robotic Angles...
            return new AngleSet();
        }
    }
}
