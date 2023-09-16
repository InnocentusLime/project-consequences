using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDirector : MonoBehaviour {
    public Shadow shadowPrefab;

    public void OnPlayerShoot(Vector2 shootDirection, Vector2 position) {
        Shadow shadow = Instantiate(
            shadowPrefab,
            position,
            Quaternion.identity);

        shadow.LookInDirection(shootDirection);
    }
}
