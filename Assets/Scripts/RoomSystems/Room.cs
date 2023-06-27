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

public class Room : MonoBehaviour {
    public static Room currentRoom;

    public GameObject player;
    public SetMadnessLevel setMadnessLevelEvent;
    public StartConsequenceTime startConsequenceTimeEvent;
    public PlayerLeaveEvent playerLeaveEvent;
    public PlayerEnterEvent playerEnterEvent;

    private void Awake() {
        if (currentRoom != null) {
            Destroy(this);
            return;
        }

        setMadnessLevelEvent ??= new SetMadnessLevel();
        startConsequenceTimeEvent ??= new StartConsequenceTime();
        playerEnterEvent ??= new PlayerEnterEvent();
        playerLeaveEvent ??= new PlayerLeaveEvent();

        currentRoom = this;
    }

    private void OnDestroy() {
        currentRoom = null;
    }
}
