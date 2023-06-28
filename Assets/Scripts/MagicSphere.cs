using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum SphereMode {
    OnShoot,
    OnInteraction,
}

public class MagicSphere : MonoBehaviour {
    public GameObject[] targets;
    public SphereMode sphereMode;

    private bool isActive;

    public void Start() {
        foreach (GameObject target in targets) {
            target.SetActive(false);
        }
    }

    private void Activate() {
        if (isActive) {
            return;
        }

        foreach (GameObject target in targets) {
            target.SetActive(true);
        }

        GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.3f);

        isActive = true;
    }

    public void OnInteract(GameObject actor) {
        if (sphereMode != SphereMode.OnInteraction) {
            return;
        }

        Activate();
    }

    public void OnBulletHit(GameObject bullet) {
        if (sphereMode != SphereMode.OnShoot) {
            return;
        }

        Activate();
    }
}
