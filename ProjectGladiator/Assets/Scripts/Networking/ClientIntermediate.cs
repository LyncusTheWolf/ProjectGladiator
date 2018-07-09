using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Gladiatorz {
    public class ClientIntermediate : NetworkBehaviour {

        private static ClientIntermediate instance;

        public static ClientIntermediate Instance {
            get { return instance; }
        }

        public void Awake() {
            if (instance != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            instance = this;
        }

        [ClientRpc]
        public void RpcUpdateClientMatchData(string matchDataString) {
            Debug.Log("Rpc Packet Recieved");
            UI_Manager.Instance.UpdateStatTextBox(matchDataString);
        }
    }
}
