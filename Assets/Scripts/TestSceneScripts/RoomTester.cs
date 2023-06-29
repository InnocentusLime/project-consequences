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

        GlobalRoomState.ResetState();
        SceneManager.UnloadSceneAsync(currentRoomScene);
    }

    private IEnumerator LoadRoom(string path) {
        AsyncOperation op = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);

        while (!op.isDone) {
            yield return op;
        }

        currentRoomScene = SceneManager.GetSceneByPath(path);

        GlobalRoomState.playerLeaveEvent.AddListener(OnRoomFinish);
        // TODO somehow learn to get the spawned player. (Or go back to the plan with a persistent player obj)
        GlobalRoomState.playerEnterEvent.Invoke(playerPrefab);
        GlobalRoomState.player.GetComponent<Gun>().playerShootEvent.AddListener(
            GetComponent<ShadowDirector>().OnPlayerShoot
            );

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
