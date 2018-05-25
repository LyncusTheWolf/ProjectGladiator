using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public class Player : Character {
        public override void Init() {
            InitializeCharacterDefaults();
            if (isLocalPlayer) {
                GameManager.Instance.BindLocalPlayer(motor, this);
            }
        }

        public override void OnDeath() {
            //throw new System.NotImplementedException();
        }

        public void OnDestroy() {
            if (isLocalPlayer) {
                GameManager.Instance.ReleaseCamera();
            }
        }
    }
}