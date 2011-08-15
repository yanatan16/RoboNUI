using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Core
{
    /**
     * <summary>
     * This class is the primary manager of the RoboNUI system.
     * It performs actions based on commands from the <see cref="VoiceCommandInterpreter"/> or the GUI, as well as manage the current on/off state of the system. 
     * It should also control the primary data path through the system, determining who the current human controller is, as well as setting the <see cref="JointAngleTranslator"/> with the correct <see cref="IRoboticModel"/> and the correct <see cref="IAngleConsumer"/>.
     * </summary>
     */
    class StateManager
    {

        //TODO implment
    }
}
