using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[Serializable]
public class PlayerShootEvent : UnityEvent<float, Vector2> {

}

[RequireComponent(typeof(CharacterPhysics),
    typeof(Gun),
    typeof(PlayerInteraction))]
public class PlayerBehaviour : CursedBehaviour {
    public bool hasGun;

    private CharacterPhysics physics;
    private Gun gun;
    private PlayerInteraction interaction;
    private Camera mainCamera;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpTakeoffSpeed;
    [SerializeField] private PlayerShootEvent playerShootEvent;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        physics = GetComponent<CharacterPhysics>();
        interaction = GetComponent<PlayerInteraction>();

        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    public void OnBulletHit(GameObject bullet) {
        gameObject.SetActive(false);
    }

    private void Update() {
        float shootingAngle = ShootingAngle();

        if (Input.GetMouseButtonDown(0) && hasGun) {
            if (gun.Shoot(shootingAngle)) {
                playerShootEvent.Invoke(shootingAngle, transform.position);
            }
        }

        if (Input.GetKey(KeyCode.Space)) {
            physics.Jump(jumpTakeoffSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            interaction.Interact();
        }

        physics.MoveHorizontally(horizontalSpeed * Input.GetAxisRaw("Horizontal"));
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
