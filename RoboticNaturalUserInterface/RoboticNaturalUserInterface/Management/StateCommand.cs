using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.Management
{
    /**
     * <summary>
     * Enumeration of types of commands
     * </summary>
     */
    enum CommandType
    {
        Activation,
        ControllerIDSelect,
        RoboticServoControllerSelect,
        SideSelection
    }

    /**
     * <summary>
     * Enumeration of types of servo controllers
     * </summary>
     */
    enum RoboticServoControllerType
    {
        Arm,
        Marionette
    }

    /**
     * <summary>
     * Enumeration of types of sides
     * </summary>
     */
    enum SideSelectionType
    {
        Right,
        Left
    }

    /**
     * <summary>
     * A command to the <see cref="StateManager"/> to affect the system.
     * </summary>
     * <remarks> Author: Jon Eisen (jon.m.eisen@gmail.com)</remarks>
     * <seealso cref="StateManager"/>
     */
    class StateCommand //TODO I'd like this to be generic but can you have a nongeneric reference to it?
    {
        /**
         * <summary>
         * Type of command this is
         * </summary>
         */
        public CommandType ComType { get; set; }

        /**
         * <summary>
         * Argument for this command
         * </summary>
         */
        public object Argument { get; set; }
    }
}
