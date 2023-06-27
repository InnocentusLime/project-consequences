using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RoomTester : MonoBehaviour {
    public GameObject playerPrefab;
    public SceneAsset[] scenes;

    private int currentRoomId;
    private Scene currentRoomScene;
    private AsyncOperation nextRoomOp;
    private string nextRoomPath;

    private void Start() {
        StartCoroutine(LoadRoom(AssetDatabase.GetAssetPath(scenes[0])));
    }

    private void UnloadCurrentRoom() {
        foreach (GameObject rootGameObject in currentRoomScene.GetRootGameObjects()) {
            rootGameObject.SetActive(false);
            Destroy(rootGameObject);
        }

        SceneManager.UnloadSceneAsync(currentRoomScene);
    }

    private IEnumerator LoadRoom(string path) {
        AsyncOperation op = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);

        while (!op.isDone) {
            yield return op;
        }

        currentRoomScene = SceneManager.GetSceneByPath(path);

        Room roomObj = FindFirstObjectByType<Room>();
        roomObj.playerLeaveEvent.AddListener(OnRoomFinish);
        roomObj.playerEnterEvent.Invoke(playerPrefab);

        yield return null;
    }

    private void OnRoomFinish() {
        UnloadCurrentRoom();

        currentRoomId += 1;
        if (currentRoomId == scenes.Length) {
            return;
        }

        StartCoroutine(LoadRoom(
            AssetDatabase.GetAssetPath(scenes[currentRoomId])
            ));
    }
}
