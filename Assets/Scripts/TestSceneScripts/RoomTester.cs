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

    private RoomState roomState;
    private int currentRoom;

    private Scene currentRoomScene;
    private AsyncOperation nextRoomOp;
    private string nextRoomPath;

    private void Awake() {
        roomState = GetComponent<RoomState>();
    }

    private void Start() {
        StartCoroutine(LoadRoom(scenes[0]));
    }

    private void UnloadCurrentRoom() {
        foreach (GameObject rootGameObject in currentRoomScene.GetRootGameObjects()) {
            rootGameObject.SetActive(false);
            Destroy(rootGameObject);
        }

        SceneManager.UnloadSceneAsync(currentRoomScene);
    }

    private IEnumerator LoadRoom(SceneAsset room) {
        string path = AssetDatabase.GetAssetPath(room);
        AsyncOperation op = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);

        while (!op.isDone) {
            yield return op;
        }

        currentRoomScene = SceneManager.GetSceneByPath(path);
        roomState.ResetState();
        RoomExitDoor exitDoor = FindFirstObjectByType<RoomExitDoor>();
        RoomEntranceDoor entranceDoor = FindFirstObjectByType<RoomEntranceDoor>();
        exitDoor.playerLeaveEvent.AddListener(OnRoomFinish);
        entranceDoor.playerEnterEvent.Invoke(player);

        yield return null;
    }

    private void OnRoomFinish(GameObject actor) {
        UnloadCurrentRoom();

        currentRoom += 1;
        if (currentRoom == scenes.Length) {
            return;
        }

        StartCoroutine(LoadRoom(scenes[currentRoom]));
    }
}
