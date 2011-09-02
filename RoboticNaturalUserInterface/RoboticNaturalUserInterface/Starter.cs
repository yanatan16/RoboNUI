using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RoboNui.Management;

namespace RoboNui
{
    /**
     * <summary>
     * This class acts as a starter for the RoboNui system.
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     * 
     */
    class Starter
    {
        /**
         * <summary>Main method for RobotNui</summary>
         */
        static void Main(string[] args)
        {
            string configFile = "robonui.json";

            StateConfiguration cfg = StateConfiguration.ReadConfigFile(configFile);
            StateManager sm = new StateManager(cfg);
            sm.Initialize();
        }
    }
}