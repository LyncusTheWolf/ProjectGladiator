using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    [RequireComponent(typeof(CharacterMotor))]
    public abstract class Character : DamagableObject {

        public uint attack;
        public uint defense;

        protected CharacterMotor motor;

        protected void InitializeCharacterDefaults() {
            motor = GetComponent<CharacterMotor>();
        }
    }
}
