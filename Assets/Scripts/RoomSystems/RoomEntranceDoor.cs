using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class PlayerEnterEvent : UnityEvent<GameObject> {}

public class RoomEntranceDoor : MonoBehaviour {
    public PlayerEnterEvent playerEnterEvent;

    private void Awake() {
        playerEnterEvent ??= new PlayerEnterEvent();
        playerEnterEvent.AddListener(OnPlayerEnter);
    }

    private void OnPlayerEnter(GameObject actor) {
        Debug.Log("Entering a room through a door");

        actor.transform.localPosition = transform.localPosition;
        actor.SetActive(true);
    }
}
