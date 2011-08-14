﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * Robotic Model Interface
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * Provides the capability to translate from a human model of Joints to a Robotic model of Angles
     */
    interface IRoboticModel
    {
        ///<summary>
        ///Translate between a human Joints model to a Robotic angle model
        ///
        ///Parameter: Joints Set (human Kinect Model)
        ///Returns: Angle set (robotic model)
        ///</summary>
        public AngleSet Translate(JointSet js);
    }
}