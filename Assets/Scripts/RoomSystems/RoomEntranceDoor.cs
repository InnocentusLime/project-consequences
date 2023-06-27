using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomEntranceDoor : MonoBehaviour {
    public void OnPlayerEnter(GameObject playerPrefab) {
        Room.currentRoom.player = Instantiate(playerPrefab, transform.localPosition, Quaternion.identity);
    }
}
