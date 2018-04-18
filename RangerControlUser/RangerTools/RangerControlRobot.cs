/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 17/04/2018
 */

using System;
using System.IO.Ports;

namespace RangerTools
{
    public class RangerControlRobot
    {
        private SerialPort comPort;

        public RangerControlRobot(string portName = "COM1", int baudRate = 9600)
        {
            this.comPort = new SerialPort();
            this.comPort.WriteBufferSize = 2;
            this.comPort.BaudRate = baudRate;
            this.comPort.PortName = portName;
        }

        ~RangerControlRobot()
        {
            this.disconnect();
        }

        public void connect()
        {
            try
            {
                this.comPort.Open();
            }
            catch(Exception e)
            {
                throw new Exception("Impossible de se connecter au Robot");
            }
        }

        public void disconnect()
        {
            this.comPort.Close();
        }

        public void setCommand(ControlStates state)
        {
            String command = ((int)state).ToString();
            if ((int)state < 10)
              command = "0" + command;
            this.comPort.Write(command);
            Console.WriteLine("Read : "+this.comPort.ReadLine());
        }
    }
}
