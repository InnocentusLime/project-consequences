using System.Linq;
using UnityEngine;

public abstract class DebugTool : MonoBehaviour {
    public abstract string toolName { get; }

    public abstract void ShowUi();
}

public class DebugToolset : MonoBehaviour {
    private int curr;
    private static DebugToolset instance;
    private string[] debugToolsNames;
    private DebugTool[] debugTools;

    private void Awake() {
        // Singleton invariant
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);

        // Actual initialization
        debugTools = GetComponents<DebugTool>();
        debugToolsNames = debugTools.Select(x => x.toolName).ToArray();
    }

    private void OnGUI() {
        curr = GUILayout.Toolbar(curr, debugToolsNames);
        debugTools[curr].ShowUi();
    }
}
