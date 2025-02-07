﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPrefab : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;

    private void Start() {
        if (!isLocalPlayer) {
            for(int i = 0; i < componentsToDisable.Length; i++) {
                componentsToDisable[i].enabled = false;
            }
        }
    }
}
