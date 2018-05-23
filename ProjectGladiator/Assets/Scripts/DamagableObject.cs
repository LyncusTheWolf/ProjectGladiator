using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public abstract class DamagableObject : NetworkBehaviour {

        public delegate void OnDeathDelegate();
        public event OnDeathDelegate onDeathEvent;

        public int currentHealth;
        public int maxHealth;

        public void Start() {
            if (isLocalPlayer) {
                currentHealth = maxHealth;
                Init();
            }
        }

        public void AdjustHealth(int amt) {
            currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
        }

        public void Kill() {
            OnDeath();
            onDeathEvent();
        }

        public abstract void Init();
        public abstract void OnDeath();
    }
}
