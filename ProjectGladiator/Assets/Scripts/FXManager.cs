using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public class FXManager : NetworkBehaviour {
        private static FXManager instance;

        public GameObject defaultHitParticle;

        public static FXManager Instance {
            get { return instance; }
        }

        public void Awake() {
            if (instance != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            instance = this;
            //Init();
        }

        [ClientRpc]
        public void RpcSpawnParticle(int particleID, Vector3 position, Quaternion rotation) {
            //TODO: Set up some form of look up table for weapon particle hits
            //TODO: Extend this into some form of object pooling system

            GameObject go = Instantiate(defaultHitParticle, position, rotation);
        }
    }
}
