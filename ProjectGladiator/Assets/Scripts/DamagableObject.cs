using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    public abstract class DamagableObject : NetworkBehaviour {

        public delegate void OnModifyDelegate(DamagableObject self);
        public event OnModifyDelegate onDeathEvent;
        public event OnModifyDelegate onDamageEvent;

        [SyncVar]
        public int currentHealth;
        [SyncVar]
        public int maxHealth;

        [SyncVar]
        protected uint totalDefense;

        public int Defense {
            get { return (int)totalDefense; }
        }

        public void Start() {
            currentHealth = maxHealth;                

            Init();
        }

        public float PollHealthRatio() {
            return currentHealth / (float)maxHealth;
        }

        [Command]
        public void CmdAdjustHealth(int amt) {
            Debug.Log("Pushing Damage");

            currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);

            RpcAdjustCallback();

            if (currentHealth == 0) {
                OnDeath();
            }
        }

        [ClientRpc]
        public void RpcAdjustCallback() {
            if (isLocalPlayer) {
                onDamageEvent(this);
            }
        }

        public void Kill() {
            OnDeath();
            onDeathEvent(this);
        }

        public abstract void Init();
        public abstract void OnDeath();
    }
}
