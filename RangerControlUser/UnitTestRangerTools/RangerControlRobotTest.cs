using RangerTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace UnitTestRangerTools
{
    [TestClass]
    public class RangerControlRobotTest
    {
        [TestMethod]
        public void TestOfControl()
        {
            RangerControlRobot rangerControlRobot = new RangerControlRobot("COM13", 9600);
            rangerControlRobot.connect();
            rangerControlRobot.setCommand(ControlStates.TRACKS_FORWARD);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.TRACK_TURN_LEFT);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.TRACKS_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_UP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_DOWN);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ARM_STOP);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.GRIPPER_CLOSE);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.GRIPPER_OPEN);
            Thread.Sleep(1000);
            rangerControlRobot.setCommand(ControlStates.ALL_STOP);
            rangerControlRobot.disconnect();
        }
    }
}
