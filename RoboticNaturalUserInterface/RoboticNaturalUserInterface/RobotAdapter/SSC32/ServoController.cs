using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace RoboNui.RobotAdapter.SSC32
{
    /**
     * Servo Controller
     * 
     * Author: Jon Eisen (yanatan16@gmail.com)
     * 
     * Base class for communicating commands to an SSC-32 servo controller
     */
    class ServoController
    {
        /**
         * Serial port communicating on
         */
        private SerialPort port;
        
        /**
         * Constructor
         * 
         * Construct class with port name and open the port
         * 
         * Parameter: port name
         */
        protected ServoController(string portName)
        {
            port = new SerialPort(portName);
            port.Open();
        }

        /**
         * Destructor
         * 
         * Close the port
         */
        protected ~ServoController()
        {
            port.Close();
        }

        /**
         * Send a command to the port
         * 
         * Parameter: Command to send
         * Returns: byte array if command has a response (length equal to com.getResponseLength())
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
