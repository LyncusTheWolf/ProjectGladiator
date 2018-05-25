using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public struct CharacterInput {
        public float leftHorizontal;
        public float leftVertical;
        public Vector3 worldMotionDirection;
        public bool jumpDown;
        public bool firing;
    }

    [System.Serializable]
    public struct CharacterBasicStats {
        public float moveSpeed;
        public float moveSpeedAcceleration;
        public float strafeSpeed;
        public float turnSpeed;
    }

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Character))]
    public class CharacterMotor : NetworkBehaviour {

        public const float INPUT_DEADZONE = 0.05f;

        public CharacterAnimator characterAnimator;
        public Transform focalPoint;
        public CharacterBasicStats stats;

        private CharacterInput frameInput;
        private ControllerState currentCharacterState;
        private Character characterStats;
        private new Rigidbody rigidbody;

        [SyncVar]
        private float motorMoveMagnitude;

        private Vector3 utilityVector;

        #region Accesors
        public Rigidbody CharacaterRigidbody {
            get { return rigidbody; }
        }

        public float MoveMagnitude {
            get { return motorMoveMagnitude; }
            set { motorMoveMagnitude = value; }
        }

        public CharacterInput CurrentFrameInput {
            get { return frameInput; }
        }
        #endregion

        public void Awake() {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Use this for initialization
        void Start() {
            GetInitialControlState();
        }

        // Update is called once per frame
        void Update() {
            if (!isLocalPlayer) {
                return;
            }

            ClearFrameInput();
            ProcessFrameInput();
            currentCharacterState.OnStateUpdate(this, frameInput, characterStats);
        }

        public void BindCharacterStats(Character charStats) {
            characterStats = charStats;
        }

        public void GetInitialControlState() {
            GameManager.Instance.PushMotorState(this, ControllerStateID.Locomotion);
        }

        public void BindState(ControllerState state) {
            if(currentCharacterState != null) {
                currentCharacterState.OnStateExit(this);
            }
            currentCharacterState = state;
            state.OnStateEnter(this);
        }

        public void ClearFrameInput() {
            frameInput.jumpDown = false;
            frameInput.worldMotionDirection = Vector3.zero;
        }

        /// <summary>
        /// Wrapper for the processing of frame input, this could be overwritten in an AI motor to allow it to utilize the same functionality
        /// </summary>
        public void ProcessFrameInput() {
            frameInput.leftHorizontal = Input.GetAxis("Horizontal");
            frameInput.leftVertical = Input.GetAxis("Vertical");
            frameInput.firing = Input.GetButton("Fire1");
            frameInput.worldMotionDirection = GetCameraRelativeInput(new Vector2(frameInput.leftHorizontal, frameInput.leftVertical), Camera.main.transform);
        }

        public Vector3 GetCameraRelativeInput(Vector2 input, Transform camera) {
            Vector3 rootDirection = transform.forward;
            Vector3 stickDirection = new Vector3(input.x, 0.0f, input.y);

            Vector3 camDir = camera.transform.forward;
            camDir.y = 0;
            Quaternion relativeRotation = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(camDir));

            Vector3 moveDirection = relativeRotation * stickDirection;

            return moveDirection;
        }
    }
}
