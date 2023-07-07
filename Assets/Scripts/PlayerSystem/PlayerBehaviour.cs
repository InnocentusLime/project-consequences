using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public class PlayerShootEvent : UnityEvent<float, Vector2> {

}

[RequireComponent(typeof(PlayerMovement), typeof(Gun))]
public class PlayerBehaviour : CursedBehaviour {
    private PlayerMovement movement;
    private Gun gun;
    private Camera mainCamera;

    [SerializeField] private PlayerShootEvent playerShootEvent;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        movement = GetComponent<PlayerMovement>();

        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    public void OnBulletHit(GameObject bullet) {
        gameObject.SetActive(false);
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
            playerShootEvent.Invoke(shootingAngle, transform.position);
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
