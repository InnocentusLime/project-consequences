using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Gun : MonoBehaviour {
    public bool hasGun = true;
    public float cooldownDuration = 0.8f;
    public GameObject bulletPrefab;

    private bool isCoolingDown;
    private Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    private void Update() {
         if (Input.GetMouseButtonDown(0) && CanShoot()) {
            isCoolingDown = true;
            StartCoroutine(CooldownRoutine(cooldownDuration));

            float shootingAngle = ShootingAngle();
            Instantiate(
                bulletPrefab,
                transform.localPosition,
                Quaternion.AngleAxis(shootingAngle, Vector3.forward)
            );
         }
    }

    private bool CanShoot() => hasGun && !isCoolingDown;

    private float ShootingAngle() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return Vector2.SignedAngle(
            Vector2.up,
            mousePosition - (Vector2)transform.localPosition
            );
    }

    private IEnumerator CooldownRoutine(float duration) {
        yield return new WaitForSeconds(duration);
        isCoolingDown = false;
    }
}
