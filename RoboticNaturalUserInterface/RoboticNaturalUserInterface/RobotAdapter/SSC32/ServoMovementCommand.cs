using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * A servo command for servo movement to an SSC-32 servo controller
     * 
     * Base class: Servo Command Group
     * </summary>
     * <remarks>
     * Author: Jon Eisen (yanatan16@gmail.com)
     * </remarks>
     * <seealso cref="ServoCommandGroup"/>
     */
    class ServoMovementCommand : ServoCommandGroup
    {
        /**
         * <summary>List of channel numbers to command (motor)</summary>
         * <remarks>Range: 0 - 31</remarks>
         */
        private List<uint> Channel;

        /**
         * <summary>List of pulse widths to command (position)</summary>
         * <remarks>
         * In units of microseconds.
         * Range: 0 - 3000, 1500 is middle
         * </remarks>
         */
        private List<ulong> PulseWidth;

        /**
         * <summary>List of movement speeds to command, optional</summary>
         * <remarks>
         * In units of microseconds per second
         * Only limits speed, may go slower if <see cref="TotalTime"/> denotes.
         * </remarks>
         */
        private List<ulong> MoveSpeed; 

        /**
         * <summary>Total time for entire movement command group</summary>
         * <remarks>
         * In unites of milliseconds
         * Only limits speed, may go slower if <see cref="MoveSpeed"/> denotes
         * </remarks>
         */
        public ulong TotalTime { get; set; }

        /**
         * <summary>
         * Constructor for this class.
         * Instantiates a ServoMovement command from base class.
         * </summary>
         */
        public ServoMovementCommand() :
            base(ServoCommandGroup.ServoCommandType.ServoMovement)
        {
            Channel = new List<uint>();
            PulseWidth = new List<ulong>();
            MoveSpeed = new List<ulong>();
            TotalTime = 0;
        }

        /**
         * <summary>
         * Add a servo movement command to the command group
         * </summary>
         * <param name="ch">Channel number</param>
         * <param name="pw">Pulse width</param>
         * <param name="ms">movement speed (optional)</param>
         */
        public void addServoMovementCommand(uint ch, ulong pw, ulong ms = 0)
        {
            NumCommands++;
            Channel.Add(ch);
            PulseWidth.Add(pw);
            MoveSpeed.Add(ms);
        }

        /**
         * <summary>
         * See <see cref="ServoCommandGroup.IncCommandString"/> for inherited method summary.
         * </summary>
         */
        protected string ServoCommandGroup.IncCommandString(int i)
        {
            string ret = string.Empty;
            //Required parameters
            ret += string.Format("#%ud P%uld", Channel[i], PulseWidth[i]);
            if (MoveSpeed[i] > 0)
                ret += string.Format(" S%uld", MoveSpeed[i]);
            return ret;
        }

        /**
         * <summary>
         * See <see cref="ServoCommandGroup.PostCommandString"/> for inherited method summary.
         * </summary>
         */
        protected string ServoCommandGroup.PostCommandString()
        {
            if (TotalTime > 0)
                return string.Format("T%uld", TotalTime);
            else
                return string.Empty;
        }
    }
}
