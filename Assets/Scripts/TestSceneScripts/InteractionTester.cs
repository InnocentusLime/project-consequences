using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTester : MonoBehaviour {
    public void Start() {
        RoomState.startConsequenceTimeEvent.AddListener(() => Debug.Log("Shid"));
    }

    public void OnInteraction(GameObject actor) {
        Debug.Log("OH MY! I WAS TOUCHED BY " + actor.name);
        RoomState.startConsequenceTimeEvent.Invoke();
    }
}
