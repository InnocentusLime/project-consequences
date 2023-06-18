using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTester : MonoBehaviour {
    void OnInteract(GameObject interactor) {
        Debug.Log("OH MY! I WAS TOUCHED BY " + interactor.name);
    }
}
