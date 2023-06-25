using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    private RoomExitDoor roomToUnlock;

    public void Awake() {
        roomToUnlock = FindFirstObjectByType<RoomExitDoor>();
    }

    public void OnInteraction(GameObject actor) {
        gameObject.SetActive(false);
        roomToUnlock.Unlock();
        RoomState.startConsequenceTimeEvent.Invoke();
    }
}
