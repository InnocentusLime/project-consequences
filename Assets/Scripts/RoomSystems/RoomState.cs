using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SetMadnessLevel : UnityEvent<int> {}

[Serializable]
public class StartConsequenceTime : UnityEvent {}

public class RoomState : MonoBehaviour {
    public static SetMadnessLevel setMadnessLevelEvent;
    public static StartConsequenceTime startConsequenceTimeEvent;

    void Awake() {
        setMadnessLevelEvent ??= new SetMadnessLevel();
        startConsequenceTimeEvent ??= new StartConsequenceTime();
    }
}
