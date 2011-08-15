using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Core;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class provides extended voice control functionality for the RoboNUI system. 
     * It communicates solely with the State Manager for the purpose of commanding an action of the system.
     * </summary>
     */
    class VoiceControlInterpreter
    {
        /**
         * <summary>
         * State Manager to send commands to
         * </summary>
         */
        private StateManager StMgr;

        /**
         * <summary>
         * Constructor requiring <see cref="StateManager"/>.
         * </summary>
         * <param name="sm">State Manager to correspond with</param>
         */
        public VoiceControlInterpreter(StateManager sm)
        {
            StMgr = sm;
        }
    }
}
