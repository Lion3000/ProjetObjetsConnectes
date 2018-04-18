/* ======================================================
* Auteur : Alex Zarzitski
* Date : 03/04/2018
*/

using ProjetObjetsConnectes;
using RangerControlUser;
using System;
using System.Net;

namespace RangerTools
{
    class CameraBoundary
    {
        private bool isOn = false;
        private string ip = "";
        private MainWindow mainWindow;
        private WebRequest request;

        public CameraBoundary(MainWindow mainWindow, string ip = "192.168.43.1")
        {
            this.ip = ip;
            this.mainWindow = mainWindow;
            this.request = WebRequest.Create("http://" + this.ip + ":8080");
        }

        ~CameraBoundary()
        {
            isOn = false;
        }

        public void start()
        {
            this.testConnection();
            this.checkConnection();
        }

        private void checkConnection()
        {
            if (this.isOn)
                this.mainWindow.showVideo(this.ip);
        }

        private void testConnection()
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    this.isOn = false;
                    this.mainWindow.hideVideo();
                    throw new CameraException("Cammera IP: " + this.ip + " n'est pas disponible");
                }
                else
                    this.isOn = true;
            }
            catch(Exception e)
            {
                this.isOn = false;
                this.mainWindow.hideVideo();
                throw new CameraException("Cammera IP: " + this.ip + " n'est pas disponible");
            }
        }
    }
}
