using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntranceDoor {
    void Enter();
}

public class ExitDoor : MonoBehaviour {
    private Room room;
    [SerializeField, SerializeReference] public IEntranceDoor adjacentDoor;

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

    private void Interact() {
        Debug.Log("Interacting with a door");

        room.Disable();
        adjacentDoor.Enter();
    }
}
