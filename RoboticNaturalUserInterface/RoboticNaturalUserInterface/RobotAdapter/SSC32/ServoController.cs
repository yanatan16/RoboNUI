using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using log4net;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * <summary>
     * Base class for communicating commands to an SSC-32 servo controller
     * </summary>
     * <remarks>Author: Jon Eisen (yanatan16@gmail.com)</remarks>
     */
    class ServoController
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
        
        /**
         * <summary>
         * Constructor with port name and open the port
         * </summary>
         * 
         * <param name="portName">Name of the serial port</param>
         */
        protected ServoController(string portName)
        {
            log = LogManager.GetLogger(this.GetType());
            log.Debug(this.ToString() + " constructed.");

            port = new SerialPort(portName);
            port.Open();

            port.ReadTimeout = 1000;
        }

        /**
         * <summary>
         * Destructor of the class. 
         * Closes the serial port.
         * </summary>
         */
        ~ServoController()
        {
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
            port.Write(com.CommandString());
            if (com.ResponseLength > 0)
            {
                byte[] buf = new byte[com.ResponseLength];
                try
                {
                    port.Read(buf, 0, (int)com.ResponseLength);
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
