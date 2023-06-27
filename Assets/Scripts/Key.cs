using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {
    private RoomExitDoor doorToUnlock;

    private void Awake() {
        doorToUnlock = FindFirstObjectByType<RoomExitDoor>();
    }

    public void OnInteraction(GameObject actor) {
        Destroy(gameObject);
        doorToUnlock.Unlock();
        GlobalRoomState.startConsequenceTimeEvent.Invoke();
    }
}
