using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RoomTester : MonoBehaviour {
    [Header("Scenes")]
    public GameObject player;
    public SceneAsset[] scenes;

    private Room[] rooms;
    private int currentRoom;
    private FinishEntranceDoor finishEntranceDoor;

    private void Awake() {
        finishEntranceDoor = GetComponent<FinishEntranceDoor>();
    }

    private void Start() {
        StartCoroutine(LoadScenesAsync());
    }

    private IEnumerator LoadScenesAsync() {
        string[] paths = scenes
            .Select(AssetDatabase.GetAssetPath)
            .ToArray();
        AsyncOperation[] sceneOperations = paths
            .Select(path => SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive))
            .ToArray();

        foreach (AsyncOperation sceneOperation in sceneOperations) {
            while (!sceneOperation.isDone) {
                yield return sceneOperation;
            }
        }

        rooms = paths.Select(SceneManager.GetSceneByPath)
            .Select(scene => scene.GetRootGameObjects()[0])
            .Select(obj => obj.GetComponent<Room>())
            .ToArray();

        // Room stitching
        foreach (Room room in rooms.Take(rooms.Length - 1)) {
            room.exitDoor.playerLeaveEvent.AddListener(OnRoomFinish);
        }
        rooms.Last().exitDoor.playerLeaveEvent.AddListener(finishEntranceDoor.OnPlayerEnter);

        // Start
        rooms.First().entranceDoor.playerEnterEvent.Invoke(player);

        yield return null;
    }

    private void OnRoomFinish(GameObject actor) {
        currentRoom += 1;
        rooms[currentRoom].entranceDoor.playerEnterEvent.Invoke(player);
    }
}
