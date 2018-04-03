using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RangerTools {
    class LeapBoundary {
        public enum Modes {
            ARM,
            TRACKS,
            IDLE
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
            get;
            set;
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
                    if(LeftFist > 0.5f)
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
                    if(RightFist > 0.5f)
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
                if(HandCount == 1)
                    return Modes.ARM;
                else if(HandCount > 1)
                    return Modes.TRACKS;
                else
                    return Modes.IDLE;
            }
        }

        private LeapBoundary () {
            HandCount = 0;

            LeftRotationAnalogue = 0f;
            LeftActive = false;
            LeftFist = 0f;

            RightRotationAnalogue = 0f;
            RightActive = false;
            RightFist = 0f;
        }

        private HandRotation angleToRotation (float angle) {
            if(angle > 7.5f)
                return HandRotation.VERTICAL;
            else if(angle > 2.5)
                return HandRotation.HORIZONTAL;
            else
                return HandRotation.INVALID;
        }
    }
}
