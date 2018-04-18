/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 17/04/2018
 */

using System;
using System.IO.Ports;

namespace RangerTools
{
    /// <summary>
    /// Cette classe controle le robot via l'enum ControlStates
    /// </summary>
    public class RangerControlRobot
    {
        private SerialPort comPort;

        /// <summary>
        /// Le constructeur initialise la classe SerialPort avec le parametre portName
        /// </summary>
        /// <param name="portName">Cette parametre defini le port COM du robot</param>
        public RangerControlRobot(string portName = "COM1")
        {
            this.comPort = new SerialPort();
            this.comPort.WriteBufferSize = 2;
            this.comPort.Parity = Parity.Even;
            this.comPort.StopBits = StopBits.One;
            this.comPort.DataBits = 8;
            this.comPort.BaudRate = 9600;
            this.comPort.PortName = portName;
        }

        /// <summary>
        /// Le deconstructeur deconnect le robot
        /// </summary>
        ~RangerControlRobot()
        {
            this.disconnect();
        }

        /// <summary>
        /// Cette methoe tante d'etablere une comunication avec le robot
        /// </summary>
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

        /// <summary>
        /// Cette methode deconnect le robot
        /// </summary>
        public void disconnect()
        {
            this.comPort.Close();
        }

        /// <summary>
        /// Cette methode envoit les instructions à executé par le robot
        /// </summary>
        public void setCommand(ControlStates state)
        {
            String command = ((int)state).ToString();
            if ((int)state < 10)
              command = "0" + command;
            this.comPort.Write(command);
        }
    }
}
