using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : CursedBehaviour {
    private Gun gun;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
    }

    public void OnBulletHit(GameObject bullet) {
        Destroy(gameObject);
    }

    protected override void OnConsequenceTime() {
        gun.hasGun = false;
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new System.NotImplementedException();
    }
}
