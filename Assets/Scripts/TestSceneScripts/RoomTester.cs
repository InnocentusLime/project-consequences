using System.Collections;
using RoomSys;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace TestSceneScripts {
    public class RoomTester : MonoBehaviour {
        public SceneAsset[] scenes;

        private int currentRoomId;
        private Scene currentRoomScene;
        private AsyncOperation nextRoomOp;
        private string nextRoomPath;

        private void Start() {
            GlobalRoomState.playerLeaveEvent.AddListener(OnRoomFinish);

            if (scenes.Length == 0) {
                GlobalRoomState.playerEnterEvent.Invoke();
            } else {
                StartCoroutine(LoadRoom(AssetDatabase.GetAssetPath(scenes[0])));
            }
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
            Assert.IsTrue(SceneManager.SetActiveScene(currentRoomScene));

            GlobalRoomState.playerEnterEvent.Invoke();

            yield return null;
        }

        private void OnRoomFinish() {
            if (scenes.Length == 0) {
                return;
            }

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
}
