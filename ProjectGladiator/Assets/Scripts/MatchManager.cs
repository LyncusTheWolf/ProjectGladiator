using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//TODO: Remove temporary serialization
[System.Serializable]
public struct MatchStatistics {
    public int kills;
    public int deaths;
}

namespace Gladiatorz {
    public class MatchManager : NetworkBehaviour {
        private static MatchManager instance;

        public float firingMaxDistance;
        public LayerMask ignoreLayer;

        public Dictionary<Player, MatchStatistics> playersInScene;

        public static MatchManager Instance {
            get { return instance; }
        }

        public void Awake() {
            if (instance != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }

            instance = this;
            Init();
        }

        public void Init() {
            playersInScene = new Dictionary<Player, MatchStatistics>();
        }

        [Server]
        public void RegisterPlayer(Player newPlayer) {
            if (playersInScene.ContainsKey(newPlayer)) {
                //TODO: Set up some form of server side error checking
                return;
            }

            playersInScene.Add(newPlayer, new MatchStatistics());
        }

        [Server]
        public string PullServerMatchData() {
            string matchString = "";

            foreach(KeyValuePair<Player, MatchStatistics> pms in playersInScene) {
                matchString += pms.Key.name + ": Kills-" + pms.Value.kills + " Deaths-" + pms.Value.deaths + " \n";
            }

            return matchString;
        }

        [Server]
        public void PerformFiringCalculations(Character characterObj, Vector3 position, Vector3 direction, LayerMask damageMask) {
            //Set the character object to a temporary layer for calculation purposes
            LayerMask temp = characterObj.gameObject.layer;
            characterObj.gameObject.layer = ignoreLayer;

            Debug.DrawRay(position, direction * firingMaxDistance);

            RaycastHit hit;
            if (Physics.Raycast(position, direction, out hit, firingMaxDistance, damageMask)) {
                DamagableObject dmg = hit.collider.GetComponent<DamagableObject>();

                Debug.Log(hit.collider.name);

                if(dmg != null) {
                    Debug.Log("Extracted damagable object");
                    int damageValue = Mathf.Clamp(characterObj.Attack - dmg.Defense, 1, 9999);

                    Debug.Log("Dealing " + damageValue);

                    dmg.CmdAdjustHealth(-damageValue);
                    //Network.Instantiate(defaultHitParticle, hit.point, Quaternion.LookRotation(hit.normal), 0);

                    FXManager.Instance.RpcSpawnParticle(-1, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            characterObj.gameObject.layer = temp;
        }

        void OnPlayerConnected(NetworkPlayer player) {
            Debug.Log("Player " + " connected from " + player.ipAddress + ":" + player.port);
        }
    }   
}
