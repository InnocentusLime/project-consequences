using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public enum PlayerState {
    Normal,
    Dead,
    Haunted,
}

[Serializable]
public class PlayerShootEvent : UnityEvent<float, Vector2> {

}

[RequireComponent(typeof(PlayerInteraction))]
public class PlayerBehaviour : CharacterBehaviour<PlayerState> {
    private static readonly Dictionary<PlayerState, StateFlags> stateFlagsMapImpl = new(){
        { PlayerState.Normal , new StateFlags {
            gun = true,
            sightMask = 0,
            reportMask = 0,
            physics = true,
        }},
        { PlayerState.Haunted , new StateFlags {
            gun = false,
            sightMask = 0,
            reportMask = 0,
            physics = true,
        }},
        { PlayerState.Dead , new StateFlags {
            gun = false,
            sightMask = 0,
            reportMask = 0,
            physics = false,
        }},
    };

    protected override Dictionary<PlayerState, StateFlags> stateFlagsMap => stateFlagsMapImpl;

    private PlayerInteraction interaction;
    private Camera mainCamera;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpTakeoffSpeed;
    [SerializeField] private PlayerShootEvent playerShootEvent;

    protected override PlayerState DefaultState() => PlayerState.Normal;

    public override bool ShouldJump() => Input.GetKey(KeyCode.Space);

    public override float GetWalkSpeed() => horizontalSpeed * Input.GetAxisRaw("Horizontal");

    public override float GetJumpSpeed() => jumpTakeoffSpeed;

    public override void OnWalk(WalkType walkType) {}

    public override void OnSuccessfulJump() {}

    public override void OnGroundChange(Vector2 groundNormal, int offGroundTicks) {}

    public override void Damage(DamageType damageType) {
        SetState(PlayerState.Dead);
    }

    public override void OnSeenObject(GameObject obj) {
        throw new NotImplementedException();
    }

    protected override void Awake() {
        base.Awake();

        interaction = GetComponent<PlayerInteraction>();

        mainCamera = Camera.main;
        Assert.IsNotNull(mainCamera);
    }

    private void Update() {
        float shootingAngle = ShootingAngle();

        if (Input.GetMouseButtonDown(0)) {
            // if (gun.Shoot(shootingAngle)) {
            //     playerShootEvent.Invoke(shootingAngle, transform.position);
            // }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            interaction.Interact();
        }
    }

    private float ShootingAngle() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        return Vector2.SignedAngle(Vector2.up, mousePosition - (Vector2) transform.localPosition);
    }

    protected override void OnConsequenceTime() {
        SetState(PlayerState.Haunted);
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new NotImplementedException();
    }
}
