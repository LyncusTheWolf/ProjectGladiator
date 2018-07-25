using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public class Player : Character {
        protected override void InitInternal() {
            InitializeCharacterDefaults();
            if (isLocalPlayer) {
                GameManager.Instance.BindLocalPlayer(motor, this);
                CmdRegisterSelfToServer();
            }
        }

        [Command]
        private void CmdRegisterSelfToServer() {
            MatchManager.Instance.RegisterPlayer(this);
        }

        public override void OnDeath() {
            Debug.Log("I have been killed");
        }

        [Server]
        private void PollServerForDeath() {
            //MatchManager.Instance
        }

        public void OnDestroy() {
            if (isLocalPlayer) {
                GameManager.Instance.ReleaseCamera();
            }
        }
    }
}