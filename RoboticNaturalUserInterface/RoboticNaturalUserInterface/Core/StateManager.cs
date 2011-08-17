using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace RoboNui.Core
{
    /**
     * <summary>
     * This class is the primary manager of the RoboNUI system.
     * It performs actions based on commands from the <see cref="T:RoboNui.KinectAdapter.VoiceCommandInterpreter"/> or the GUI, as well as manage the current on/off state of the system. 
     * It should also control the primary data path through the system, determining who the current human controller is, as well as setting the <see cref="JointAngleTranslator"/> with the correct <see cref="T:RoboNui.Core.IRoboticModel"/> and the correct <see cref="T:RoboNui.Core.IAngleConsumer"/>.
     * </summary>
     */
    class StateManager
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        public StateManager()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");
        }

        static void Main(string [] args)
        {

        }
        //TODO implment
    }
}
