using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public abstract class ControllerState {
        public abstract ControllerStateID GetControllerStateID();

        public virtual void OnStateEnter(CharacterMotor motor) { }
        public virtual void OnStateExit(CharacterMotor motor) { }

        /// <summary>
        /// The update function called each frame by the character motor, transitions should be handled within the update near the end of the function
        /// </summary>
        /// <param name="motor"></param>
        /// <param name="input"></param>
        /// <param name="characterBase"></param>
        public abstract void OnStateUpdate(CharacterMotor motor, CharacterInput input, Character characterBase);
    }

    public class GameManager : MonoBehaviour{

        private static GameManager instance;

        //TODO: Move this declaration to a different object later
        public GameObject sceneCamera;

        public GameObject playerCameraPrefab;

        private CameraRig playerCam;
        private Player playerRef;

        private static Dictionary<ControllerStateID, ControllerState> controllerStateDictionary;

        public static GameManager Instance {
            get { return instance; }
        }

        public void Awake() {
            if (instance != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            instance = this;
            Init();
        }

        public void Init() {
            BuildControllerDictionary();
        }

        public void BindLocalPlayer(CharacterMotor motor, Player player) {
            if(playerCam == null) {
                GameObject go = GameObject.Instantiate(playerCameraPrefab) as GameObject;
                playerCam = go.GetComponent<CameraRig>();
            }            
            playerCam.ResetCamera(motor);
            sceneCamera.SetActive(false);

            playerRef = player;
            player.onDamageEvent -= UpdateCamera;
            player.onDamageEvent += UpdateCamera;
        }

        public void UpdateCamera(DamagableObject dmgObj) {
            UI_Manager.Instance.SetHealthRect(dmgObj.PollHealthRatio());
        }

        public void ReleaseCamera() {
            if(playerCam == null) {
                return;
            }

            playerCam.gameObject.SetActive(false);

            playerRef.onDamageEvent -= UpdateCamera;
            playerRef = null;
        }

        public void ToggleMouseSettings(bool state) {
            if (state) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            } else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void BuildControllerDictionary() {
            AddControllerState(new LocomotionState());
            AddControllerState(new FiringState());
        }

        private void AddControllerState(ControllerState state) {
            if (controllerStateDictionary == null) {
                controllerStateDictionary = new Dictionary<ControllerStateID, ControllerState>();
            }

            ControllerStateID id = state.GetControllerStateID();
            if (controllerStateDictionary.ContainsKey(id)) {
                return;
            } else {
                controllerStateDictionary.Add(id, state);
            }
        }

        public void PushMotorState(CharacterMotor motor, ControllerStateID id) {
            if (controllerStateDictionary.ContainsKey(id) == false) {
                Debug.LogWarning("Warning, no definition for the state of id " + id + " exist within the current state dictionary");
                return;
            }

            motor.BindState(controllerStateDictionary[id]);
        }
    }
}