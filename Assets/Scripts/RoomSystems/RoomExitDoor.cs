using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class PlayerLeaveEvent : UnityEvent<GameObject> {}

public class RoomExitDoor : MonoBehaviour {
    public PlayerLeaveEvent playerLeaveEvent;

    private void Awake() {
        playerLeaveEvent ??= new PlayerLeaveEvent();
    }

    public void OnInteract(GameObject actor) {
        Debug.Log("Interacting with a door");

        actor.SetActive(false);
        playerLeaveEvent.Invoke(actor);
    }
}
