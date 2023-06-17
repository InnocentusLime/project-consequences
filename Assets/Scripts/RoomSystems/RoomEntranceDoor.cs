using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntranceDoor : MonoBehaviour, IEntranceDoor {
    private Room room;

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

    public void Enter() {
        room.Enable();
    }
}
