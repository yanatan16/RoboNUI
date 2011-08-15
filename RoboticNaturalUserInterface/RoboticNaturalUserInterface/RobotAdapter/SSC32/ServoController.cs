using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

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
            port = new SerialPort(portName);
            port.Open();
        }

        /**
         * <summary>
         * Destructor of the class. 
         * Closes the serial port.
         * </summary>
         */
        protected ~ServoController()
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
                port.Read(buf, 0, (int) com.ResponseLength);
                return buf;
            }
            else
                return null; 
        }

    }
}
