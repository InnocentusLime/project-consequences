// TODO make breakable pod

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Venus : CursedBehaviour {
    public Collider2D eatCollider;
    private readonly Collider2D[] objectsToEat = new Collider2D[3];
    private ContactFilter2D eatFilter;
    private bool isActive = false;
    public GameObject display;

    protected override void ExtraAwake() {
        eatFilter.useLayerMask = true;
        eatFilter.layerMask = LayerMask.GetMask("Entities");
        display.SetActive(false);
    }


    private void Update() {
        if (!isActive) {
            return;
        }

        int count = eatCollider.OverlapCollider(eatFilter, objectsToEat);
        for (int i = 0; i < count; i++) {
            GameObject objectToEat = objectsToEat[i].gameObject;
            if (objectToEat == GlobalRoomState.player) {
                Destroy(objectToEat);
                continue;
            }
            if (objectToEat.GetComponent<ZombieBehaviour>() != null) {
                Destroy(objectToEat);
                Destroy(gameObject);
            }

        }
    }

    protected override void OnConsequenceTime() {
        isActive = true;
        display.SetActive(true);
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new System.NotImplementedException();
    }
}
