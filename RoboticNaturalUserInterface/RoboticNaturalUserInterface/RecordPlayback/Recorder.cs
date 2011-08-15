using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.RecordPlayback
{
    /**
     * <summary>
     * This class records angles sent to the <see cref="IRoboticAngleConsumer"/> and stores it for later.
     * 
     * Interface: <see cref="IRoboticAngleConsumer"/>
     * </summary>
     * <seealso cref="RoboticAngleProvider"/>
     */
    class Recorder : IRoboticAngleConsumer
    {
        /**
         * <summary>See <see cref="IRoboticAngleConsumer.UpdateAngles"/> for the inherited method summary</summary>
         */
        void IRoboticAngleConsumer.UpdateAngles(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
