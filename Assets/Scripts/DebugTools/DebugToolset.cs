using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class DebugTool : MonoBehaviour
{
    public abstract string Name { get; }

    public abstract void ShowUi();
}

public class DebugToolset : MonoBehaviour
{
    private int Curr = 0;
    private static DebugToolset Instance;
    private string[] DebugToolsNames;
    private DebugTool[] DebugTools;

    private void Awake()
    {
        // Singleton invariant
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        // Actual initialization
        DebugTools = GetComponents<DebugTool>();
        DebugToolsNames = DebugTools.Select(x => x.Name).ToArray();
    }

    private void OnGUI()
    {
        Curr = GUILayout.Toolbar(Curr, DebugToolsNames);
        DebugTools[Curr].ShowUi();
    }
}
