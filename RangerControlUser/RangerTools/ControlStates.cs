/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 17/03/2018
 */
namespace RangerTools
{
    /// <summary>
    /// Cette emun definit les actions du robot
    /// </summary>
    public enum ControlStates
    {
        TRACKS_FORWARD,
        TRACKS_BACKWARD,
        TRACK_TURN_LEFT,
        TRACK_TURN_RIGHT,
        TRACKS_STOP,
        ARM_UP,
        ARM_DOWN,
        ARM_STOP,
        GRIPPER_OPEN,
        GRIPPER_CLOSE,
        GRIPPER_STOP,
        ALL_STOP = 12
    }
}
