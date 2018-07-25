using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

namespace Gladiatorz {
    public class GladiatorNetworkManager : NetworkManager {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        private bool CheckNetworkStatus() {
            return (!NetworkClient.active && !NetworkServer.active && singleton.matchMaker == null);
        }

        public void GladiatorsStartHost() {
            if (CheckNetworkStatus()) {
                singleton.StartHost();
            } else {
                Debug.LogError("Could not initialize server");
            }
        }

        public void GladiatorsStartClient() {
            if (CheckNetworkStatus()) {
                singleton.StartClient();
            } else {
                Debug.LogError("Could not connect to a host");
            }
        }

        public override void OnServerConnect(NetworkConnection conn) {
            Debug.Log("A client connected to the server: " + conn);
        }

        public override void OnServerReady(NetworkConnection conn) {

            NetworkServer.SetClientReady(conn);

            Debug.Log("Client is set to the ready state (ready to receive state updates): " + conn);

            //ClientScene.AddPlayer(conn, 0);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
            //base.OnServerAddPlayer(conn, playerControllerId);

            Transform t = GetStartPosition();
            GameObject playerObj = (GameObject)GameObject.Instantiate(playerPrefab, t.position, t.rotation);

            //Player p = playerObj.GetComponent<Player>();
            //p.maxHealth = 5000;
    
            NetworkServer.AddPlayerForConnection(conn, playerObj, playerControllerId);
        }

        public override void OnClientConnect(NetworkConnection conn) {
            //base.OnClientConnect(conn);

            ClientScene.Ready(conn);
            ClientScene.AddPlayer(conn, 0);
        }
    }
}
