using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

using Utilities.Messaging;

namespace RoboNui.RecordPlayback
{
    /**
     * <summary>
     * This class reads in stored angle data for robotic servos and forwards them to the <see cref="T:RoboNui.Messaging.IConsumer"/> denoted.
     * 
     * Base Class: <see cref="Provider{AngleSet}"/>
     * </summary>
     */
    class Playback : Provider<AngleSet>
    {
        public Playback() :
            base()
        {

        }
    }
}
