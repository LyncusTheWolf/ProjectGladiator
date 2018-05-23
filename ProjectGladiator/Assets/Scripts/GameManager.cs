using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class GameManager : MonoBehaviour{

        private static GameManager instance;

        //TODO: Move this declaration to a different object later
        public GameObject sceneCamera;

        public GameObject playerCameraPrefab;

        public LayerMask quickFireMask;
        public float firingMaxDistance;

        private Player playerList;
        private Enemy enemyList;

        private CameraRig playerCam;

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

            //TODO: Set up a server ID list
        }

        public void PerformFiringCalculations(Vector3 position, Vector3 direction) {
            RaycastHit hit;
            if (Physics.Raycast(position, direction, out hit, firingMaxDistance, quickFireMask)) {

            }
        }

        public void InitCamera(CharacterMotor motor) {
            if(playerCam == null) {
                GameObject go = GameObject.Instantiate(playerCameraPrefab) as GameObject;
                playerCam = go.GetComponent<CameraRig>();
            }            
            playerCam.ResetCamera(motor, true);
            sceneCamera.SetActive(false);
            //ToggleMouseSettings(false);
        }

        public void ReleaseCamera() {
            playerCam.gameObject.SetActive(false);
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
    }
}