using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    [RequireComponent(typeof(Collider))]
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

        protected Collider objCollider;

        public int Defense {
            get { return (int)totalDefense; }
        }

        public void Start() {
            objCollider = GetComponent<Collider>();

            Init();
        }

        public void Init() {
            currentHealth = maxHealth;

            InitInternal();
        }

        public float PollHealthRatio() {
            return currentHealth / (float)maxHealth;
        }

        [Server]
        public void AdjustHealth(int amt) {
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

        protected abstract void InitInternal();
        public abstract void OnDeath();
    }
}
