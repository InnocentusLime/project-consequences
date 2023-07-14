// TODO make breakable pod

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Venus : CursedBehaviour {
    private EyeSight eyesight;
    public GameObject display;

    protected override void ExtraAwake() {
        eyesight = GetComponent<EyeSight>();
        display.SetActive(false);

        eyesight.enabled = false;
    }

    public void OnSeeingObject(GameObject objectToEat) {
        if (objectToEat == GlobalRoomState.player) {
            Destroy(objectToEat);
            return;
        }

        if (objectToEat.GetComponent<ZombieBehaviour>() != null) {
            Destroy(objectToEat);
            Destroy(gameObject);
        }
    }

    protected override void OnConsequenceTime() {
        display.SetActive(true);
        eyesight.enabled = true;
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new System.NotImplementedException();
    }
}
