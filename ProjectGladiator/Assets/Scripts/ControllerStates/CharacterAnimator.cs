using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class CharacterAnimator : MonoBehaviour {

        private CharacterMotor rootMotor;

        public void InitAnimator(CharacterMotor rootMotor) {
            this.rootMotor = rootMotor;
        }
    }
}
