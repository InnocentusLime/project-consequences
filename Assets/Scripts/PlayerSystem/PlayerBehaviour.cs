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

public class PlayerBehaviour : CursedBehaviour {
    private PlayerMovement movement;
    private Gun gun;
    private Camera mainCamera;

    public PlayerShootEvent playerShootEvent;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        movement = GetComponent<PlayerMovement>();

        // TODO set explicitly
        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    public void OnBulletHit(GameObject bullet) {
        Destroy(gameObject);
    }

    private void Update() {
        bool shot = false;
        float shootingAngle = ShootingAngle();

        if (Input.GetMouseButtonDown(0)) {
            shot = gun.Shoot(shootingAngle);
        }

        if (Input.GetKey(KeyCode.Space)) {
            movement.Jump();
        }

        movement.MoveHorizontally(Input.GetAxisRaw("Horizontal"));

        if (shot) {
            var shootConfiguration = new ShootConfiguration {
                angle = shootingAngle,
                position = transform.position
            };

            playerShootEvent.Invoke(shootConfiguration);
        }
    }

    private float ShootingAngle() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return Vector2.SignedAngle(Vector2.up, mousePosition - (Vector2) transform.localPosition);
    }

    protected override void OnConsequenceTime() {
        gun.hasGun = false;
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new NotImplementedException();
    }
}
