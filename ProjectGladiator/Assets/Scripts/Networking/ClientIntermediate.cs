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

        [Client]
        public void RequestMatchData() {
            CmdPullServerData();
        }

        [Command]
        private void CmdPullServerData() {
            string  pms = MatchManager.Instance.PullServerMatchData();
            TargetUpdateMatchData(connectionToClient, pms);
        }

        [TargetRpc]
        public void TargetUpdateMatchData(NetworkConnection target, string newData) {
            UI_Manager.Instance.UpdateStatTextBox(newData);
        }
    }
}
