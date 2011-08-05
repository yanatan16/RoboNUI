using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNUI.Core
{
    /**
     * Joint-Angle Translator
     * 
     * This component translates between Kinect Joints and Robotic Angles
     * After being called with new Joints' positions, the JAT forwards them
     * to the currently active IRoboticAngleConsumer, which is set by the
     * State Manager.
     *
     * Base Class: RoboticAngleProvider
     */
    class JointAngleTranslator : RoboticAngleProvider
    {
    }
}
