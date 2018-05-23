using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class Player : Character {
        public override void Init() {
            InitializeCharacterDefaults();
            GameManager.Instance.InitCamera(motor);
        }

        public override void OnDeath() {
            throw new System.NotImplementedException();
        }

        public void OnDestroy() {
            GameManager.Instance.ReleaseCamera();
        }
    }
}