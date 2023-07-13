using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SetMadnessLevel : UnityEvent<int> {
}

[Serializable]
public class StartConsequenceTime : UnityEvent {
}

[Serializable]
public class PlayerLeaveEvent : UnityEvent {
}

[Serializable]
public class PlayerEnterEvent : UnityEvent {
}

public class GlobalRoomState : MonoBehaviour {
    public static GameObject player;
    public static SetMadnessLevel setMadnessLevelEvent = new();
    public static StartConsequenceTime startConsequenceTimeEvent = new();
    public static PlayerLeaveEvent playerLeaveEvent = new();
    public static PlayerEnterEvent playerEnterEvent = new();

    private void Awake() {
        player = FindFirstObjectByType<PlayerBehaviour>(FindObjectsInactive.Include).gameObject;
        setMadnessLevelEvent ??= new SetMadnessLevel();
        startConsequenceTimeEvent ??= new StartConsequenceTime();
        playerEnterEvent ??= new PlayerEnterEvent();
        playerLeaveEvent ??= new PlayerLeaveEvent();
    }
}
