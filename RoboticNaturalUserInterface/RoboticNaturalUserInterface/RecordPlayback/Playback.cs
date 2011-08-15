using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RecordPlayback
{
    /**
     * <summary>
     * This class reads in stored angle data for robotic servos and forwards them to the <see cref="IRoboticAngleConsumer"/> denoted.
     * 
     * Base Class: <see cref="RoboticAngleProvider"/>
     * </summary>
     */
    class Playback : RoboticAngleProvider
    {
        public Playback() :
            base()
        {

        }
    }
}
