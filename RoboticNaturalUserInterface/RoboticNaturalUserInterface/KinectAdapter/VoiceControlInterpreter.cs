﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

using RoboNui.Core;
using RoboNui.Management;

using Utilities.Messaging;

namespace RoboNui.KinectAdapter
{
    /**
     * <summary>
     * This class provides extended voice control functionality for the RoboNUI system. 
     * It communicates solely with the State Manager for the purpose of commanding an action of the system.
     * </summary>
     * <remarks>Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     */
    class VoiceControlInterpreter : Provider<StateCommand>
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        /**
         * <summary>
         * Constructor
         * </summary>
         */
        public VoiceControlInterpreter() :
            base()
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");
        }
    }
}
