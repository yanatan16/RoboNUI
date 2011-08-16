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
     * This class records angles sent to the <see cref="T:IConsumer"/> and stores it for later.
     * 
     * Interface: <see cref="T:RoboNui.Messaging.IConsumer"/> with T = <see cref="AngleSet"/>
     * </summary>
     * <seealso cref="T:Provider{AngleSet}"/>
     */
    class Recorder : IConsumer<AngleSet>
    {
        /**
         * <summary>See <see cref="M:IConsumer.Update"/> for the inherited method summary</summary>
         */
        void IConsumer<AngleSet>.Update(AngleSet angles)
        {
            throw new NotImplementedException();
        }
    }
}
