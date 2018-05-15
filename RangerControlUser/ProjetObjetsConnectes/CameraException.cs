/* ======================================================
* Auteur : Alex Zarzitski
* Date : 03/04/2018
*/
using System;

namespace ProjetObjetsConnectes
{
    /// <summary>
    /// Cette classe définit une exeption de la camèra
    /// </summary>
    class CameraException : Exception
    {
        public CameraException(string message) : base(message)
        {
        }
    }
}
