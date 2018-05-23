using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gladiatorz {
    public static class TraitDatabase {

        private static Dictionary<uint, Trait> traitDictionary;

        public static Trait GetTrait(uint id) {
            if (traitDictionary == null) {
                InitDatabase();
            }

            if (traitDictionary.ContainsKey(id) == false) {
                Debug.LogWarning("Warning, trait with ID: " + id + " does not exist within the database, returning null trait");
                return traitDictionary[0];
            }

            Trait targetTrait = traitDictionary[id];
            if (targetTrait == null) {
                Debug.LogWarning("Warning, trait with ID: " + id + " does not exist within the database, returning null trait");
                return traitDictionary[0];
            } else {
                return targetTrait;
            }
        }

        private static void Foo(uint level) {

        }

        private static void InitDatabase() {
            traitDictionary = new Dictionary<uint, Trait>();

            //Bind in the empty trait so that the dictionary has a debugging value
            BindTrait(0, new Trait(
                "Empty",
                1,
                Foo)
            );
        }

        private static void BindTrait(uint id, Trait trait) {
            if (traitDictionary.ContainsKey(id) == false) {
                Debug.LogError("Error, " + traitDictionary[id].TraitName + " already exist within the databse with id " + id + " cannot insert item " + trait.TraitName);
                return;
            }

            traitDictionary[id] = trait;
        }
    }
}
