using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour {
    private void Start() {
        Disable();
    }

    public void Enable() {
        // Works well enough for now
        gameObject.SetActive(true);
    }

    public void Disable() {
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
        if (room == null) {
            return null;
        }

        return room;
    }
}
