using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class CameraRig : MonoBehaviour {

        public const float DEAD_ZONE = 0.005f;

        public CharacterMotor motor;

        public float startingZoom;
        public float zoomMinClamp = 5.0f;
        public float zoomMaxClamp = 20.0f;

        public Vector3 lookOffset;

        public float easing = 5.0f;
        public float yawEasing = 5.0f;

        public float yawControlSpeed = 3.0f;
        public float pitchControlSpeed = 3.0f;
        public float mouseSensitivity = 30.0f;

        [Range(0.0f, 1.0f)]
        public float rotBias = 1.0f;

        public float startingPitch = 15.0f;
        public float pitchMinClamp = -15.0f;
        public float pitchMaxClamp = 60.0f;

        public bool resetOnNoMotion;

        [Header("Occlusion Controls")]
        public LayerMask occlusionMask;
        public float normalOffset;

        private static CameraRig instance;

        private float currentYaw;
        private float currentPitch;
        private float currentZoom;
        private Transform followTarget;

        private Vector3 smoothVel;
        private Vector3 currentMousePos;

        public static CameraRig Instance {
            get { return instance; }
        }

        public Vector3 FlatSpaceDirection() {
            Vector3 outDir = transform.forward;
            outDir.y = 0.0f;
            return outDir.normalized;
        }

        void Awake() {
            if(instance != null) {
                Destroy(gameObject);
                return;
            }

            instance = this;

            //ResetCamera(motor, true);
        }

        public void Update() {
            CameraControl(Time.deltaTime);

            if (Input.GetButtonDown("R3")) {
                ResetCamera(motor, true);
            }
        }

        // Update is called once per frame
        void FixedUpdate() {
            //CameraControl(Time.fixedDeltaTime);
        }

        private void CameraControl(float timeDelta) {
            float cameraHorizontal = 0.0f;
            float cameraVertical = 0.0f;
            //float dHorizontal = Input.GetAxis("D Horizontal");
            float zoomDelta = 0.0f;

            GetCameraMovement(ref cameraHorizontal, ref cameraVertical, ref zoomDelta);

            currentZoom = Mathf.Clamp(currentZoom + (zoomDelta * timeDelta * 3.0f), zoomMinClamp, zoomMaxClamp);

            //currentYaw = Mathf.LerpAngle(currentYaw, currentYaw + cameraHorizontal * yawControlSpeed, timeDelta);

            currentYaw = (currentYaw + cameraHorizontal * yawControlSpeed * timeDelta) % 360.0f;

            //currentPitch = Mathf.LerpAngle(currentPitch, currentPitch + cameraVertical * pitchControlSpeed, timeDelta);

            currentPitch = Mathf.Clamp(currentPitch + cameraVertical * pitchControlSpeed * timeDelta, pitchMinClamp, pitchMaxClamp);

            Vector3 targetDir = -(Quaternion.Euler(currentPitch, currentYaw, 0.0f) * Vector3.forward) * currentZoom;
            Vector3 targetPosition = followTarget.position + targetDir;

            targetDir.Normalize();
            Vector3 lookDir = -targetDir;
                        
            targetDir.y = 0.0f;
            targetPosition += Quaternion.LookRotation(targetDir) * lookOffset;

            PushPosition(targetPosition, timeDelta);
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        private void GetCameraMovement(ref float cameraHorizontal, ref float cameraVertical, ref float zoomDelta) {
            Vector3 newMousePos = Input.mousePosition;

            cameraHorizontal += (newMousePos.x - currentMousePos.x);
            cameraVertical += -(newMousePos.y - currentMousePos.y);

            currentMousePos = newMousePos;

            cameraHorizontal += -Input.GetAxis("RightHorizontal");
            cameraVertical += -Input.GetAxis("RightVertical");

            zoomDelta += Input.mouseScrollDelta.y;
            zoomDelta += Input.GetAxis("D Vertical");
        }

        public void PushPosition(Vector3 targetPos, float timeDelta) {

            RaycastHit hit;

            Ray hitRay = new Ray(followTarget.position, targetPos - followTarget.position);

            Debug.DrawRay(followTarget.position, targetPos - followTarget.position, Color.blue);

            if (Physics.Raycast(hitRay, out hit, currentZoom, occlusionMask, QueryTriggerInteraction.Ignore)) {
                transform.position = hit.point + hit.normal * normalOffset;
            } else {
                transform.position = targetPos;
            }

        }

        public void ResetCamera(CharacterMotor newMotor, bool snapToPosition) {
            if (newMotor != null) {
                motor = newMotor;
                followTarget = motor.focalPoint;
            }

            currentYaw = followTarget.transform.rotation.eulerAngles.y;
            currentPitch = startingPitch;
            currentZoom = startingZoom;

            currentMousePos = Input.mousePosition;

            if (snapToPosition) {
                transform.position = followTarget.position - (Quaternion.Euler(currentPitch, currentYaw, 0.0f) * Vector3.forward) * startingZoom;
                transform.LookAt(followTarget);
            }
        }
    }
}
