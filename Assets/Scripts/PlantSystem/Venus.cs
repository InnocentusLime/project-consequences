// TODO make breakable pod

using System;
using UnityEngine;

public class Venus : CursedBehaviour {
    private IDamageable damageable;
    private EyeSight eyesight;
    public GameObject display;

    protected override void Awake() {
        base.Awake();

        damageable = GetComponent<IDamageable>();
        eyesight = GetComponent<EyeSight>();
        display.SetActive(false);

        eyesight.enabled = false;
    }

    public void OnSeeingObject(GameObject objectToEat) {
        if (!objectToEat.TryGetComponent(out Edible edible)) {
            return;
        }

        edible.GetEaten(DamageType.VenusEat);
        switch (edible.GetFoodType()) {
            case FoodType.Harmless:
                break;
            case FoodType.Poisonous:
                damageable.Damage(DamageType.FoodPoison);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
