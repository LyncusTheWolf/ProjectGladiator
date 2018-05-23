using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public abstract class ControllerState {
        public CharacterMotor parentMotor;

        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public abstract void OnStateUpdate(CharacterInput input);
    }

    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMotor : MonoBehaviour {

        public const float INPUT_DEADZONE = 0.05f;

        public CharacterAnimator characterAnimator;
        public Transform focalPoint;
        public CharacterBasicStats stats;

        private CharacterInput frameInput;
        private ControllerState currentCharacterState;
        private new Rigidbody rigidbody;

        private Vector3 utilityVector;

        #region Accesors
        public Rigidbody CharacaterRigidbody {
            get { return rigidbody; }
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
            BuildControllerStates();
        }

        // Update is called once per frame
        void Update() {
            ClearFrameInput();
            ProcessFrameInput();
            currentCharacterState.OnStateUpdate(frameInput);
        }

        public void BuildControllerStates() {
            currentCharacterState = new LocomotionState();
            currentCharacterState.parentMotor = this;
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
