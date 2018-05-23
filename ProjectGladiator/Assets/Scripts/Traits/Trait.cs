using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public class Trait {

        public delegate void ApplyTraitDelegate(uint currentLevel);

        private ApplyTraitDelegate onApplyTrait;

        #region Accessors
        public string TraitName { get; private set; }

        public uint MaxTraitLevel { get; private set; }
        #endregion


        public Trait(string traitName, uint maxTraitLevel, ApplyTraitDelegate applyTraitDelegate) {
            TraitName = traitName;
            MaxTraitLevel = maxTraitLevel;
            onApplyTrait = applyTraitDelegate;
        }

        public void ApplyTrait(uint traitLevel) {
            if (traitLevel == 0) {
                Debug.LogError("Error, level zero passed as a parameter to trait " + TraitName);
            }

            onApplyTrait(traitLevel);
        }
    }
}
