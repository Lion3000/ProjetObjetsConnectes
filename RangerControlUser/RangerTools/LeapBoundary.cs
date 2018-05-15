/**
 * \author Ivaylo Dimitrov 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangerTools {
    /// <summary>
    /// This singleton class takes in parameters detected by the LEAP Motion hardware
    /// and converts them into understandable and usable states by offering a
    /// developper friendly and user friendly layer of values and methods
    /// </summary>
    public class LeapBoundary {
        /// <summary>
        /// The different modes dependant on number of hands present
        /// in the field of vision and their angle of rotation
        /// </summary>
        public enum Modes {
            ARM,
            GRIP,
            TRACKS,
            IDLE
        }

        /// <summary>
        /// The main hand identifiers
        /// </summary>
        public enum HandId {
            LEFT,
            RIGHT
        }

        /// <summary>
        /// The possible states of a given hand
        /// </summary>
        public enum HandState {
            FIST,
            PALM,
            INACTIVE
        }

        /// <summary>
        /// The possible rotations of a given hand
        /// </summary>
        public enum HandRotation {
            HORIZONTAL,
            VERTICAL,
            INVALID
        }

        private static LeapBoundary instance = null;

        public static LeapBoundary Instance {
            get {
                if(instance == null)
                    instance = new LeapBoundary();
                return instance;
            }

            private set {
                instance = value;
            }
        }

        /// <summary>
        /// Accessor property returning the number of active hands
        /// </summary>
        public int HandCount {
            get {
                int count = 0;
                if(LeftActive)
                    count++;
                if (RightActive)
                    count++;
                return count;
            }
        }

        /// <summary>
        /// The raw analogue rotation of the left hand, between 0.0f and 1.0f
        /// </summary>
        public float LeftRotationAnalogue {
            get;
            set;
        }

        /// <summary>
        /// The rotation of the left hand, value from the HandRotation enum
        /// </summary>
        public HandRotation LeftRotation {
            get {
                return angleToRotation( LeftRotationAnalogue );
            }
        }

        /// <summary>
        /// The raw analogue rotation of the right hand, between 0.0f and 1.0f
        /// </summary>
        public float RightRotationAnalogue {
            get;
            set;
        }

        /// <summary>
        /// The rotation of the right hand, value from the HandRotation enum
        /// </summary>
        public HandRotation RightRotation {
            get {
                return angleToRotation( RightRotationAnalogue );
            }
        }

        /// <summary>
        /// Boolean property indicating the presence of a left hand
        /// </summary>
        public bool LeftActive {
            get;
            set;
        }

        /// <summary>
        /// The raw analogue value representing the clench
        /// value of the left fist/hand, between 0.0f and 1.0f
        /// </summary>
        public float LeftFist {
            get;
            set;
        }

        /// <summary>
        /// A property making use of LeftActive and LeftFist
        /// in order to return a more readable and comprehensible
        /// state of the left hand
        /// </summary>
        public HandState LeftState {
            get {
                if(LeftActive) {
                    if(LeftFist == 1f)
                        return HandState.FIST;
                    else
                        return HandState.PALM;
                } else {
                    return HandState.INACTIVE;
                }
            }
        }

        /// <summary>
        /// Boolean property indicating the presence of a right hand
        /// </summary>
        public bool RightActive {
            get;
            set;
        }

        /// <summary>
        /// The raw analogue value representing the clench
        /// value of the right fist/hand, between 0.0f and 1.0f
        /// </summary>
        public float RightFist {
            get;
            set;
        }

        /// <summary>
        /// A property making use of RightActive and RightFist
        /// in order to return a more readable and comprehensible
        /// state of the right hand
        /// </summary>
        public HandState RightState {
            get {
                if(RightActive) {
                    if(RightFist == 1f)
                        return HandState.FIST;
                    else
                        return HandState.PALM;
                } else {
                    return HandState.INACTIVE;
                }
            }
        }

        /// <summary>
        /// A collective property making use of both hands'
        /// attributes and that returns the respective part
        /// of the robot that should be currently controlled
        /// </summary>
        public Modes Mode {
            get {
                if(HandCount == 1) {
                    HandRotation rotation;
                    if(LeftActive)
                        rotation = LeftRotation;
                    else
                        rotation = RightRotation;
                    if(rotation == HandRotation.HORIZONTAL)
                        return Modes.GRIP;
                    else if(rotation == HandRotation.VERTICAL)
                        return Modes.ARM;
                    else return Modes.IDLE;
                } else if(HandCount > 1)
                    return Modes.TRACKS;
                else
                    return Modes.IDLE;
            }
        }

        private LeapBoundary () {
            LeftRotationAnalogue = 0f;
            LeftActive = false;
            LeftFist = 0f;

            RightRotationAnalogue = 0f;
            RightActive = false;
            RightFist = 0f;
        }

        /// <summary>
        /// The entry method used to externally set
        /// the raw values needed for each hand
        /// </summary>
        /// <param name="hand">The identifier of the hand to be set</param>
        /// <param name="rotationAnalogue">The raw analogue rotation of the corresponding hand</param>
        /// <param name="fist">The raw analogue clench value of the corresponding hand</param>
        public void setHandAttributes (HandId hand, float rotationAnalogue, float fist) {
            if (hand == HandId.LEFT) {
                LeftRotationAnalogue = rotationAnalogue;
                LeftFist = fist;
            } else {
                RightRotationAnalogue = rotationAnalogue;
                RightFist = fist;
            }
        }

        /// <summary>
        /// This method is used to convert raw rotation
        /// to the user defined rotation states
        /// </summary>
        /// <param name="angle">The raw analogue rotation of a hand</param>
        /// <returns>Corresponding rotation identifier</returns>
        private HandRotation angleToRotation (float angle) {
            if(angle > 0.75f)
                return HandRotation.VERTICAL;
            else if(angle > 0.25f)
                return HandRotation.HORIZONTAL;
            else
                return HandRotation.INVALID;
        }

        /// <summary>
        /// Used to convert all internal attributes to a corresponding
        /// user defined action comprehensible by the robot
        /// </summary>
        /// <returns>The command to be sent to the robot</returns>
        public ControlStates toControlState() {
            ControlStates state = ControlStates.ALL_STOP;
            HandState handState;

            switch (this.Mode) {
                case Modes.TRACKS:
                    if (LeftState == HandState.PALM && RightState == HandState.PALM) {
                        state = ControlStates.TRACKS_FORWARD;
                    } else if (LeftState == HandState.FIST && RightState == HandState.FIST) {
                        state = ControlStates.TRACKS_BACKWARD;
                    } else if (LeftState == HandState.PALM && (RightState == HandState.INACTIVE || RightState == HandState.FIST)) {
                        state = ControlStates.TRACK_TURN_LEFT;
                    } else if (RightState == HandState.PALM && (LeftState == HandState.INACTIVE || LeftState == HandState.FIST)) {
                        state = ControlStates.TRACK_TURN_RIGHT;
                    }
                    break;
                case Modes.ARM:
                    if (LeftActive)
                        handState = LeftState;
                    else
                        handState = RightState;

                    if (handState == HandState.FIST)
                        state = ControlStates.ARM_DOWN;
                    else if (handState == HandState.PALM)
                        state = ControlStates.ARM_UP;
                    break;
                case Modes.GRIP:
                    if (LeftActive)
                        handState = LeftState;
                    else
                        handState = RightState;

                    if (handState == HandState.FIST)
                        state = ControlStates.GRIPPER_CLOSE;
                    else if (handState == HandState.PALM)
                        state = ControlStates.GRIPPER_OPEN;
                    break;
            }

            return state;
        }
    }
}
