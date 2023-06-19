using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTester : MonoBehaviour {
    public void OnInteraction(GameObject actor) {
        Debug.Log("OH MY! I WAS TOUCHED BY " + actor.name);
    }
}
