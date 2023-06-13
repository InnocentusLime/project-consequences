using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TestSceneLoader : DebugTool
{
    public override string Name => "TestSceneLoader";

    private bool Show = false;
    private Vector2 ScrollPosition;
    [SerializeField] private SceneAsset[] testRooms;

    public override void ShowUi()
    {
        if (GUILayout.Button("Toggle"))
        {
            Show = !Show;
        }

        if (!Show)
        {
            return;
        }

        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
        foreach (SceneAsset scene in testRooms)
        {
            if (GUILayout.Button(scene.name))
            {
                SceneManager.LoadScene(scene.name);
            }
        }
        GUILayout.EndScrollView();
    }
}
