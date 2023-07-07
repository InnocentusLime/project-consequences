using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class RoomTester : MonoBehaviour {
    public SceneAsset[] scenes;

    private int currentRoomId;
    private Scene currentRoomScene;
    private AsyncOperation nextRoomOp;
    private string nextRoomPath;

    private void Start() {
        StartCoroutine(LoadRoom(AssetDatabase.GetAssetPath(scenes[0])));
        GlobalRoomState.playerLeaveEvent.AddListener(OnRoomFinish);
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
        GlobalRoomState.playerEnterEvent.Invoke();

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

    private void ResetRoom() {
        UnloadCurrentRoom();

        foreach (Shadow shadow in GetComponentsInChildren<Shadow>()) {
            Destroy(shadow.gameObject);
        }

        StartCoroutine(LoadRoom(
            AssetDatabase.GetAssetPath(scenes[currentRoomId])
        ));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetRoom();
        }
    }
}
