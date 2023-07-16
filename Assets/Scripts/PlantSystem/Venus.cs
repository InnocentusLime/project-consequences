// TODO make breakable pod

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Venus : CursedBehaviour {
    private Damageable damageable;
    private EyeSight eyesight;
    public GameObject display;

    protected override void ExtraAwake() {
        damageable = GetComponent<Damageable>();
        eyesight = GetComponent<EyeSight>();
        display.SetActive(false);

        eyesight.enabled = false;
    }

    public void OnSeeingObject(GameObject objectToEat) {
        if (objectToEat.TryGetComponent(out Edible edible)) {
            switch (edible.GetFoodType()) {
                case FoodType.Harmless:
                    edible.GetEaten(DamageType.VenusEat);
                    return;
                case FoodType.Poisonous:
                    edible.GetEaten(DamageType.VenusEat);
                    damageable.Damage(DamageType.FoodPoison);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
