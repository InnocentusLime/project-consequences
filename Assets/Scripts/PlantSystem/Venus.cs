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
        Edible edible = objectToEat.GetComponent<Edible>();
        if (edible != null) {
            if (edible.GetFoodType() == FoodType.Harmless) {
                edible.GetEaten(DamageType.VenusEat);
            }
            if (edible.GetFoodType() == FoodType.Poisonous) {
                edible.GetEaten(DamageType.VenusEat);
                Damageable damageable = GetComponent<Damageable>();
                if (damageable != null) {
                    damageable.Damage(DamageType.FoodPoison);
                }
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
