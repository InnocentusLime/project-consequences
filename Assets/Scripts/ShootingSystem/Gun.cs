using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Gun : MonoBehaviour {
    public bool hasGun = true;
    public float cooldownDuration = 0.8f;
    public Bullet bulletPrefab;

    private bool isCoolingDown;

    public bool Shoot(float shootingAngle) {
        if (!CanShoot()) {
            return false;
        }

        // FIXME this ugliness exists only because the shotgun of the shadow
        // is implemented as many calls to `Shoot()`. Fix it simply by making the
        // shotgun shot its own object.
        if (!cooldownDuration.Equals(0.0f)) {
            isCoolingDown = true;
            StartCoroutine(CooldownRoutine(cooldownDuration));
        }

        Bullet bullet = Instantiate(bulletPrefab, transform.position,
            Quaternion.AngleAxis(shootingAngle, Vector3.forward));
        bullet.creator = gameObject;

        return true;
    }

    private bool CanShoot() => hasGun && !isCoolingDown;

    private IEnumerator CooldownRoutine(float duration) {
        yield return new WaitForSeconds(duration);
        isCoolingDown = false;
    }
}
