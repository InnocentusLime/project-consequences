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
public class PlayerEnterEvent : UnityEvent<GameObject> {
}

public class GlobalRoomState : MonoBehaviour {
    public static GameObject player;
    public static SetMadnessLevel setMadnessLevelEvent;
    public static StartConsequenceTime startConsequenceTimeEvent;
    public static PlayerLeaveEvent playerLeaveEvent;
    public static PlayerEnterEvent playerEnterEvent;

    public static void ResetState() {
        player = null;
        setMadnessLevelEvent = new SetMadnessLevel();
        startConsequenceTimeEvent = new StartConsequenceTime();
        playerEnterEvent = new PlayerEnterEvent();
        playerLeaveEvent = new PlayerLeaveEvent();
    }

    private void Awake() {
        ResetState();
    }
}
