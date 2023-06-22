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
    [SerializeField] private int madnessLevel;
    public SetMadnessLevel setMadnessLevelEvent;

    [SerializeField] private bool consequenceTime;
    public StartConsequenceTime startConsequenceTimeEvent;

    // Start is called before the first frame update
    void Start() {
        setMadnessLevelEvent ??= new SetMadnessLevel();
        startConsequenceTimeEvent ??= new StartConsequenceTime();
    }

    public void ResetState() {
        madnessLevel = 0;
        consequenceTime = false;
    }
}
