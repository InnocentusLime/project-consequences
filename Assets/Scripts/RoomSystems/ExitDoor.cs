using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntranceDoor {
    void Enter();
}

public class ExitDoor : MonoBehaviour {
    private Room room;
    [SerializeField, SerializeReference] private IEntranceDoor adjacentDoor;

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

        if (adjacentDoor == null) {
            Debug.LogWarning("Door's adjacent door is set to null");
        }
    }

    private void Interact() {
        room.Disable();
        adjacentDoor.Enter();
    }
}
