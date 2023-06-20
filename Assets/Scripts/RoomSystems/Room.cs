using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour {
    public RoomExitDoor exitDoor;
    public RoomEntranceDoor entranceDoor;

    private void Awake() {
        exitDoor = GetComponentInChildren<RoomExitDoor>();
        entranceDoor = GetComponentInChildren<RoomEntranceDoor>();

        entranceDoor.playerEnterEvent.AddListener(OnPlayerEnter);
        exitDoor.playerLeaveEvent.AddListener(OnPlayerLeave);
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnPlayerEnter(GameObject actor) {
        // Works well enough for now
        gameObject.SetActive(true);
    }

    private void OnPlayerLeave(GameObject actor) {
        // Works well enough for now
        gameObject.SetActive(false);
    }

    // We expect exactly one root object. And that object should have a Room component.
    public static Room FindInScene(Scene scene) {
        GameObject[] gameObjects = scene.GetRootGameObjects();
        if (gameObjects.Length != 1) {
            return null;
        }

        GameObject root = gameObjects[0];
        Assert.IsNotNull(root);

        Room room = root.GetComponent<Room>();
        return room == null ? null : room;
    }
}
