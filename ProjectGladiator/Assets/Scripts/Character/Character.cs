using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gladiatorz {
    [RequireComponent(typeof(CharacterMotor))]
    public abstract class Character : DamagableObject {

        public LayerMask quickFireMask;

        public GameObject weaponHitParticle;

        public float weaponDelay;

        [SyncVar]
        public uint baseAttack;
        [SyncVar]
        public uint baseDefense;

        [SyncVar]
        private uint totalAttack;

        [SyncVar]
        private float weaponLastFire;

        public int Attack {
            get { return (int)totalAttack; }
        }

        protected CharacterMotor motor;

        protected void InitializeCharacterDefaults() {
            totalAttack = baseAttack;
            totalDefense = baseDefense;

            motor = GetComponent<CharacterMotor>();
            motor.BindCharacterStats(this);
        }

        public void RequestQuickFireCalculation() {
            Transform tf = CameraRig.Instance.transform;
            CmdQuickFire(tf.position, tf.forward);
        }

        [Command]
        private void CmdQuickFire(Vector3 position, Vector3 direction) {
            if(weaponLastFire < Time.time - weaponDelay) {
                MatchManager.Instance.PerformFiringCalculations(this, position, direction, quickFireMask);
                weaponLastFire = Time.time;
            }
        }
    }
}
