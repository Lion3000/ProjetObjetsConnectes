using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangerTools {
    public class LeapBoundary {
        public enum Modes {
            ARM,
            GRIP,
            TRACKS,
            IDLE
        }

        public enum HandId {
            LEFT,
            RIGHT
        }

        public enum HandState {
            FIST,
            PALM,
            INACTIVE
        }

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

        public float LeftRotationAnalogue {
            get;
            set;
        }

        public HandRotation LeftRotation {
            get {
                return angleToRotation( LeftRotationAnalogue );
            }
        }

        public float RightRotationAnalogue {
            get;
            set;
        }
        
        public HandRotation RightRotation {
            get {
                return angleToRotation( RightRotationAnalogue );
            }
        }

        public bool LeftActive {
            get;
            set;
        }

        public float LeftFist {
            get;
            set;
        }

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

        public bool RightActive {
            get;
            set;
        }

        public float RightFist {
            get;
            set;
        }

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

        public void setHandAttributes (HandId hand, float rotationAnalogue, float fist) {
            if (hand == HandId.LEFT) {
                LeftRotationAnalogue = rotationAnalogue;
                LeftFist = fist;
            } else {
                RightRotationAnalogue = rotationAnalogue;
                RightFist = fist;
            }
        }

        private HandRotation angleToRotation (float angle) {
            if(angle > 0.75f)
                return HandRotation.VERTICAL;
            else if(angle > 0.25f)
                return HandRotation.HORIZONTAL;
            else
                return HandRotation.INVALID;
        }

        public ControlStates toControlState() {
            ControlStates state = ControlStates.ALL_STOP;
            HandState handState;

            switch (this.Mode) {
                case Modes.TRACKS:
                    if (LeftState == HandState.PALM && RightState == HandState.PALM) {
                        state = ControlStates.TRACKS_FORWARD;
                    } else if (LeftState == HandState.PALM && (RightState == HandState.INACTIVE || RightState == HandState.FIST)) {
                        state = ControlStates.TRACK_TURN_RIGHT;
                    } else if (RightState == HandState.PALM && (LeftState == HandState.INACTIVE || LeftState == HandState.FIST)) {
                        state = ControlStates.TRACK_TURN_LEFT;
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
