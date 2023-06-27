using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomEntranceDoor : MonoBehaviour {
    public void Awake() {
        GlobalRoomState.playerEnterEvent.AddListener(OnPlayerEnter);
    }

    private void OnDestroy() {
        GlobalRoomState.playerEnterEvent.RemoveListener(OnPlayerEnter);
    }

    private void OnPlayerEnter(GameObject playerPrefab) {
        GlobalRoomState.player = Instantiate(playerPrefab, transform.localPosition, Quaternion.identity);
    }
}
