using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CursedBehaviour : MonoBehaviour {
    private void Awake() {
        GlobalRoomState.startConsequenceTimeEvent.AddListener(OnConsequenceTime);
        GlobalRoomState.setMadnessLevelEvent.AddListener(OnMadnessChange);

        ExtraAwake();
    }

    private void OnDestroy() {
        GlobalRoomState.startConsequenceTimeEvent.RemoveListener(OnConsequenceTime);
        GlobalRoomState.setMadnessLevelEvent.RemoveListener(OnMadnessChange);

        ExtraOnDestroy();
    }

    protected abstract void OnConsequenceTime();

    protected abstract void OnMadnessChange(int madnessLevel);

    protected virtual void ExtraAwake() {}

    protected virtual void ExtraOnDestroy() {}
}
