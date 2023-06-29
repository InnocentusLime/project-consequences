using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public struct ShootConfiguration {
    public float angle;
    public Vector2 position;
}

[Serializable]
public class PlayerShootEvent : UnityEvent<ShootConfiguration> {

}

public class Gun : MonoBehaviour {
    public bool isPlayerControlled = true;
    public bool hasGun = true;
    public float cooldownDuration = 0.8f;
    public Bullet bulletPrefab;
    [SerializeField] private PlayerShootEvent playerShootEvent;

    private bool isCoolingDown;
    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);

        if (isPlayerControlled) {
            playerShootEvent ??= new PlayerShootEvent();
        }
    }

    private void Update() {
        // FIXME super-duper ugly botch
        if (Input.GetMouseButtonDown(0) && CanShoot() && isPlayerControlled) {
            Shoot(PlayerShootingAngle());
        }
    }

    public void Shoot(float shootingAngle) {
        isCoolingDown = true;
        StartCoroutine(CooldownRoutine(cooldownDuration));

        Bullet bullet = Instantiate(bulletPrefab, transform.localPosition,
            Quaternion.AngleAxis(shootingAngle, Vector3.forward));
        bullet.creator = gameObject;

        if (isPlayerControlled) {
            ShootConfiguration shootConfiguration = new ShootConfiguration {
                angle = shootingAngle,
                position = transform.localPosition
            };

            playerShootEvent.Invoke(shootConfiguration);
        }
    }

    private bool CanShoot() => hasGun && !isCoolingDown;

    private float PlayerShootingAngle() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return Vector2.SignedAngle(Vector2.up, mousePosition - (Vector2) transform.localPosition);
    }

    private IEnumerator CooldownRoutine(float duration) {
        yield return new WaitForSeconds(duration);
        isCoolingDown = false;
    }
}
