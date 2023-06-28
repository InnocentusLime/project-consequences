using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDirector : MonoBehaviour {
    public Shadow shadowPrefab;

    public void OnPlayerShoot(ShootConfiguration shootConfiguration) {
        Shadow shadow = Instantiate(
            shadowPrefab,
            shootConfiguration.position,
            Quaternion.identity,
            transform);

        shadow.shootAngle = shootConfiguration.angle;
        shadow.gameObject.SetActive(false);
    }
}
