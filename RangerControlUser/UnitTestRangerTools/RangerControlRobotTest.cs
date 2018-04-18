/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 18/04/2018
 */
 
 using RangerTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UnitTestRangerTools
{
    [TestClass]
    /// <summary>
    /// Cette classe test le bon fonctionment du robot
    /// </summary>
    public class RangerControlRobotTest
    {
        /// <summary>
        /// Cette methode test le robot. Une comunication est etablie avec le robot, 
        /// puis les differets commandes sont envoie pour verifier le bon fonctionment des moteurs.
        /// </summary>
        [TestMethod]
        public void TestOfControl()
        {
            RangerControlRobot rangerControlRobot = new RangerControlRobot("COM13");
            rangerControlRobot.connect();
            rangerControlRobot.setCommand(ControlStates.TRACKS_FORWARD);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.TRACK_TURN_LEFT);
            Thread.Sleep(3000);
            rangerControlRobot.setCommand(ControlStates.TRACK_TURN_RIGHT);
            Thread.Sleep(3000);
            rangerControlRobot.setCommand(ControlStates.TRACKS_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.TRACKS_BACKWARD);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.TRACKS_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_UP);
            Thread.Sleep(5000);
            rangerControlRobot.setCommand(ControlStates.ARM_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_DOWN);
            Thread.Sleep(2000);
            rangerControlRobot.setCommand(ControlStates.ARM_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.GRIPPER_CLOSE);
            Thread.Sleep(2000);
            rangerControlRobot.setCommand(ControlStates.GRIPPER_OPEN);
            Thread.Sleep(2000);
            rangerControlRobot.setCommand(ControlStates.ALL_STOP);
            rangerControlRobot.disconnect();
        }
    }
}
