using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneLoader : DebugTool {
    public override string Name => "TestSceneLoader";

    private bool show;
    private Vector2 scrollPosition;
    [SerializeField] private SceneAsset[] testRooms;

    public override void ShowUi() {
        if (GUILayout.Button("Toggle")) {
            show = !show;
        }

        if (!show) {
            return;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (SceneAsset scene in testRooms) {
            if (GUILayout.Button(scene.name)) {
                SceneManager.LoadScene(scene.name);
            }
        }

        GUILayout.EndScrollView();
    }
}
