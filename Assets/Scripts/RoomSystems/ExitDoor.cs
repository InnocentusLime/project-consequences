using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdjacentDoor {
    void OnPlayerEnter(GameObject player);
}

public class ExitDoor : MonoBehaviour {
    private Room room;
    public IAdjacentDoor adjacentDoor;

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

    public void OnInteract(GameObject actor) {
        Debug.Log("Interacting with a door");

        room.Disable();
        actor.SetActive(false);
        adjacentDoor.OnPlayerEnter(actor);
    }
}
