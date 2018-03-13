using System;
using System.Collections.Generic;
using System.Linq;

/* ======================================================
 * Auteur : Alex Zarzitski
 * Date : 13/03/2018
 */
namespace RangerTools
{
    public enum ControlStates
    {
        RIGHT_TRACK_STOP,
        RIGHT_TRACK_FORWARD,
        RIGHT_TRACK_BACK,
        LEFT_TRACK_STOP,
        LEFT_TRACK_FORWARD,
        LEFT_TRACK_BACK,
        ARM_UP, ARM_DOWN,
        ARM_STOP, GRIPPER_OPEN,
        GRIPPER_CLOSE,
        GRIPPER_STOP,
        ALL_STOP
    }
}
