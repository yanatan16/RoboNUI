using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.KinectAdapter;

using Microsoft.Research.Kinect.Nui;

namespace RoboNui.Core
{
    /**
     * Joint-Angle Translator
     * 
     * Base Class: RoboticAngleProvider
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * This component translates between Kinect Joints and Robotic Angles
     * After being called with new Joints' positions, the JAT forwards them
     * to the currently active IRoboticAngleConsumer, which is set by the
     * State Manager.
     */
    class JointAngleTranslator : RoboticAngleProvider
    {
        //TODO comment
        JointSet JointPositions
        {
            public set
            {
                base.SendAngles(Model.Translate(JointPositions));
            }
            private get;
        }

        IRoboticModel Model { public set; private get; }

    }
}
