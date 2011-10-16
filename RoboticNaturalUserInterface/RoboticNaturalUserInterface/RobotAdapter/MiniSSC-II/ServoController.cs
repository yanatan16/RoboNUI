using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;

using log4net;

// TODO Implement

namespace RoboNui.RobotAdapter.MiniSSC2
{
    /**
     * <summary>
     * Base class for communicating commands to an SSC-32 servo controller
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     */
    class MiniSSCIIServoController
    {
        /**
         * <summary>Log for logging events in this class</summary>
         */
        private ILog log;

        /**
         * <summary>
         * Serial port this controller communicates on
         * </summary>
         */
        private SerialPort port;

        bool inactive;

        ICollection<uint> activeChannels;

        /**
         * <summary>
         * Constructor with port name and open the port.
         * Also initializes servo channels to center
         * </summary>
         * 
         * <param name="portName">Name of the serial port</param>
         * <param name="channels">Channels to center on construction</param>
         */
        protected MiniSSCIIServoController(string portName, ICollection<uint> channels)
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            activeChannels = channels;
            try
            {
                port = new SerialPort(portName);
                port.Open();
                port.BaudRate = 9600;
                port.NewLine = string.Empty + Convert.ToChar(13);
                port.Handshake = System.IO.Ports.Handshake.None;
                port.BaseStream.Flush();

                port.ReadTimeout = 1000;
                inactive = false;

                foreach (uint ch in channels)
                {
                    ServoMovementCommand smc = new ServoMovementCommand(ch, 128);
                    sendCommand(smc);
                }
                log.Info("Initiating all servos to center.");
            }
            catch (IOException ex)
            {
                log.Error("Could not open Servo Controller Port on " + portName, ex);
                inactive = true;
            }

        }
        
        /**
         * <summary>
         * Destructor of the class. 
         * Closes the serial port.
         * </summary>
         */
        ~MiniSSCIIServoController()
        {
            if (activeChannels != null)
            {
                foreach (uint ch in activeChannels)
                {
                    ServoMovementCommand smc = new ServoMovementCommand(ch, 128);
                    sendCommand(smc);
                }
            }
            if (port != null)
            {
                port.Close();
            }
        }

        /**
         * <summary>
         * Send a command to the servo controller
         * </summary>
         * 
         * <param name="com">Command to send</param>
         */
        protected void sendCommand(ServoMovementCommand com)
        {
            string str = "Send Command 0x";
            foreach (byte b in com.CommandString())
            {
                str += string.Format("{0:00x}", b);
            }
            log.Debug(str);

            if (!inactive)
            {
                IEnumerable<byte> command = com.CommandString();
                port.Write(command.ToArray(), 0, command.Count());
            }
        }

    }
}
