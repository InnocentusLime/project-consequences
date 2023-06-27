using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public class PlayerLeaveEvent : UnityEvent<GameObject> {
}

public class RoomExitDoor : MonoBehaviour {
    [SerializeField] private bool isLocked = true;
    public PlayerLeaveEvent playerLeaveEvent;

    private void Awake() {
        playerLeaveEvent ??= new PlayerLeaveEvent();
    }

    private void Start() {
        // NOTE remove ASAP. This for a more testable prototype
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.color = isLocked ? new Color(0.0f, 0.3f, 0.0f) : new Color(0.0f, 1.0f, 0.0f);
    }

    public void OnInteract(GameObject actor) {
        if (isLocked) {
            Debug.Log("Locked");
            return;
        }

        actor.SetActive(false);
        playerLeaveEvent.Invoke(actor);
    }

    public void Unlock() {
        isLocked = false;
        GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f);
    }
}
