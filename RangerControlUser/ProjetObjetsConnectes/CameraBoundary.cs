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
    /// <summary>
    /// Cette classe contrôle le rendu de la caméra
    /// </summary>
    class CameraBoundary
    {
        private bool isOn = false;
        private string ip = "";
        private MainWindow mainWindow;
        private WebRequest request;

        /// <summary>
        /// Le constructeur initialise la gestion de la caméra
        /// </summary>
        /// <param name="mainWindow">Objet qui gére la vue de l'aplication</param>
        /// <param name="ip">Ce paramètre definit l'adrresse IP de la caméra</param>
        public CameraBoundary(MainWindow mainWindow, string ip = "192.168.43.1")
        {
            this.ip = ip;
            this.mainWindow = mainWindow;
            this.request = WebRequest.Create("http://" + this.ip + ":8080");
        }

        /// <summary>
        /// Le deconstructeur de la classe
        /// </summary>
        ~CameraBoundary()
        {
            isOn = false;
        }

        /// <summary>
        /// Cette methode démarre l'affichage de la caméra
        /// </summary>
        public void start()
        {
            this.testConnection();
            if (this.isOn)
                this.mainWindow.showVideo(this.ip);
        }

        /// <summary>
        /// Cette methode teste l'activité de la caméra en http
        /// </summary>
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
