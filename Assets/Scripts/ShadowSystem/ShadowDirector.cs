using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDirector : MonoBehaviour {
    public Shadow shadowPrefab;

    public void OnPlayerShoot(float angle, Vector2 position) {
        Shadow shadow = Instantiate(
            shadowPrefab,
            position,
            Quaternion.identity,
            transform);

        shadow.shootAngle = angle;
        shadow.gameObject.SetActive(false);
    }
}
