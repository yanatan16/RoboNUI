using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using RoboNUI.RobotAdapter.SSC32;

namespace RoboNUI.RobotAdapter.SSC32
{
    class ServoController
    {
        private SerialPort port;
        
        protected ServoController(string portName)
        {
            port = new SerialPort(portName);
            port.Open();
        }

        protected ~ServoController()
        {
            port.Close();
        }

        protected byte[] sendCommand(ServoCommandGroup com)
        {
            port.Write(com.CommandString());
            if (com.hasResponse())
            {
                byte[] buf = new byte[com.getResponseLength()];
                port.Read(buf, 0, (int) com.getResponseLength());
                return buf;
            }
            else
                return null;
            
        }

    }
}
