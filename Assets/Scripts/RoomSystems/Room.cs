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

public class Room : MonoBehaviour {
    public static Room currentRoom;
    public SetMadnessLevel setMadnessLevelEvent;
    public StartConsequenceTime startConsequenceTimeEvent;

    private void Awake() {
        setMadnessLevelEvent ??= new SetMadnessLevel();
        startConsequenceTimeEvent ??= new StartConsequenceTime();
        currentRoom = this;
    }
}
