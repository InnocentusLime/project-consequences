using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[Serializable]
public class PlayerShootEvent : UnityEvent<float, Vector2> {

}

[RequireComponent(typeof(CharacterPhysics), typeof(Gun))]
public class PlayerBehaviour : CursedBehaviour {
    public bool hasGun;

    private CharacterPhysics physics;
    private Gun gun;
    private Camera mainCamera;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpTakeoffSpeed;
    [SerializeField] private PlayerShootEvent playerShootEvent;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        physics = GetComponent<CharacterPhysics>();

        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    public void OnBulletHit(GameObject bullet) {
        gameObject.SetActive(false);
    }

    private void Update() {
        bool shot = false;
        float shootingAngle = ShootingAngle();

        if (Input.GetMouseButtonDown(0) && hasGun) {
            shot = gun.Shoot(shootingAngle);
        }

        if (Input.GetKey(KeyCode.Space)) {
            physics.Jump(jumpTakeoffSpeed);
        }

        physics.MoveHorizontally(horizontalSpeed * Input.GetAxisRaw("Horizontal"));

        if (shot) {
            playerShootEvent.Invoke(shootingAngle, transform.position);
        }
    }

    private float ShootingAngle() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return Vector2.SignedAngle(Vector2.up, mousePosition - (Vector2) transform.localPosition);
    }

    protected override void OnConsequenceTime() {
        hasGun = false;
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new NotImplementedException();
    }
}
