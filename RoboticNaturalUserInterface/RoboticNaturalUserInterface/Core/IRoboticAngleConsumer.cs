﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNUI.Core;

namespace RoboNUI.Core
{
    /**
     * Robotic Angle Consumer Interface
     * 
     * Author: Jon Eisen (jonathan.eisen@ngc.com)
     * 
     * Provide an interface for receiving updated angles from the JAT.
     */
    interface IRoboticAngleConsumer
    {
        /**
         * Update the angles to this consumer
         * 
         * Parameter: Angle set
         */
        public void updateAngles(AngleSet angles);
    }
}