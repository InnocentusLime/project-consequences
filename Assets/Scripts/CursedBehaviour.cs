using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CursedBehaviour : MonoBehaviour {
    protected virtual void Awake() {
        GlobalRoomState.startConsequenceTimeEvent.AddListener(OnConsequenceTime);
        GlobalRoomState.setMadnessLevelEvent.AddListener(OnMadnessChange);
    }

    protected virtual void OnDestroy() {
        GlobalRoomState.startConsequenceTimeEvent.RemoveListener(OnConsequenceTime);
        GlobalRoomState.setMadnessLevelEvent.RemoveListener(OnMadnessChange);
    }

    protected abstract void OnConsequenceTime();

    protected virtual void OnMadnessChange(int madnessLevel) {
        throw new NotImplementedException();
    }
}
