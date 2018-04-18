/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 17/04/2018
 */

using System;
using System.IO;
using System.Xml.Serialization;

namespace RangerTools
{
    public class Config
    {
        private string ipCamera;
        private int rangerControlBaudRate;
        private string rangerControlPortName;

        public Config()
        {
        }

        public string IpCamera { get => ipCamera; set => ipCamera = value; }
        public int RangerControlBaudRate { get => rangerControlBaudRate; set => rangerControlBaudRate = value; }
        public string RangerControlPortName { get => rangerControlPortName; set => rangerControlPortName = value; }

        public void getConfig()
        {
            string testData = "";
            try
            {
                testData = File.ReadAllText(@"config.ini");
            }
            catch (Exception e)
            {
                testData = @"<Config><IpCamera>192.168.43.1</IpCamera><RangerControlBaudRate>9600</RangerControlBaudRate><RangerControlPortName>COM1</RangerControlPortName></Config>";
                File.WriteAllText(@"config.ini", testData);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                using (TextReader reader = new StringReader(testData))
                {
                    Config result = (Config)serializer.Deserialize(reader);
                    this.IpCamera = result.IpCamera;
                    this.RangerControlBaudRate = result.RangerControlBaudRate;
                    this.RangerControlPortName = result.RangerControlPortName;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Le fichier de configuration n'est pas au bon format");
            }

        }
    }
}
