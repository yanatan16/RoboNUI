using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;

using log4net;


namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * Base class for communicating commands to an SSC-32 servo controller
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     */
    class SSC32ServoController
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
        protected SSC32ServoController(string portName, ICollection<uint> channels)
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            try
            {
                port = new SerialPort(portName);
                port.Open();
                port.BaudRate = 115200;
                port.NewLine = string.Empty + Convert.ToChar(13);
                port.Handshake = System.IO.Ports.Handshake.None;
                port.BaseStream.Flush();

                port.ReadTimeout = 1000;
                inactive = false;
                activeChannels = channels;

                ServoMovementCommand smc = new ServoMovementCommand();
                foreach (uint ch in channels)
                {
                    smc.addServoMovementCommand(ch, 1500);
                }
                log.Info("Initiating all servos to center.");
                sendCommand(smc);
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
        ~SSC32ServoController()
        {
            if (activeChannels != null)
            {
                ServoMovementCommand smc = new ServoMovementCommand();
                foreach (uint ch in activeChannels)
                {
                    smc.addServoMovementCommand(ch, 1500);
                }
            }
            port.Close();
        }

        /**
         * <summary>
         * Send a command to the servo controller
         * </summary>
         * 
         * <param name="com">Command to send</param>
         * <returns>
         * Byte array if command has a response.
         * Length of byte array is com.getResponseLength())
         * </returns>
         */
        protected byte[] sendCommand(ServoCommandGroup com)
        {
            string command = com.CommandString();
            //log.Debug("Send Command " + command);

            if (!inactive)
            {
                port.WriteLine(command);
            }
            if (com.ResponseLength > 0)
            {
                byte[] buf = new byte[com.ResponseLength];
                try
                {
                    if (inactive)
                    {
                        buf = Enumerable.Repeat<byte>((byte)0, (int) com.ResponseLength).ToArray<byte>();
                    }
                    else
                    {
                        port.Read(buf, 0, (int)com.ResponseLength);
                    }
                }
                catch (TimeoutException te)
                {
                    log.Error("Timeout exception on read to SSC32.", te);
                    return null;
                }
                return buf;
            }
            else
                return null; 
        }

    }
}
