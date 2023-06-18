using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    private Room room;
    public GameObject adjacentDoor;

    private void Awake() {
        GameObject parent = transform.parent.gameObject;

        if (parent == null) {
            Debug.LogError("Door has no parent");
            return;
        }

        room = parent.GetComponent<Room>();

        if (room == null) {
            Debug.LogError("Door's parent isn't a room");
        }
    }

    private void OnInteract(GameObject interactor) {
        Debug.Log("Interacting with a door");

        room.Disable();
        adjacentDoor.SendMessage("OnPlayerEnter", interactor);
    }
}
