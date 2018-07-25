using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class BreakableObject : DamagableObject {
        protected override void InitInternal() {
            //throw new System.NotImplementedException();
        }

        public override void OnDeath() {
            gameObject.SetActive(false);
        }
    }
}
