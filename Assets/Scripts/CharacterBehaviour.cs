using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StateFlags {
    public bool gun;
    public LayerMask sightMask;
    public LayerMask reportMask;
    public bool physics;
}

[RequireComponent(typeof(Eyesight), typeof(CharacterPhysics))]
public abstract class CharacterBehaviour<T> : CursedBehaviour, IDamageable, IEyesightClient,
    ICharacterPhysicsController {
    // Components
    private Gun gun;
    private CharacterPhysics physics;

    // Private state fields
    private T currentState;

    // Interface
    [SerializeField] protected bool hasGun;
    private IEyesightClient eyesightClientImplementation;

    /* Required methods */

    protected virtual Dictionary<T, StateFlags> stateFlagsMap { get; } = null;
    protected abstract T DefaultState();
    public abstract bool ShouldJump();
    public abstract float GetWalkSpeed();
    public abstract float GetJumpSpeed();

    /* Event handlers */

    public abstract void Damage(DamageType damageType);
    public abstract void OnSeenObject(GameObject obj);
    public abstract bool OnWalk(WalkType walkType);
    public abstract void OnSuccessfulJump();
    public abstract void OnLand(Vector2 groundNormal, int offGroundTicks);

    /* Child API */

    protected override void Awake() {
        base.Awake();

        gun = GetComponent<Gun>();
        physics = GetComponent<CharacterPhysics>();

        currentState = DefaultState();
        ApplyFlags(stateFlagsMap[currentState]);
    }

    protected void SetState(T newState) {
        if (currentState.Equals(newState)) {
            return;
        }

        currentState = newState;
        ApplyFlags(stateFlagsMap[currentState]);
    }

    protected bool Shoot(float angle) => hasGun && gun.Shoot(angle);

    /* Private API */

    private void ApplyFlags(StateFlags flags) {
        // Gun
        hasGun = flags.gun;
        gun.enabled = flags.gun;

        // Physics
        physics.enabled = flags.physics;
    }

    public LayerMask GetSightMask() => stateFlagsMap[currentState].sightMask;

    public LayerMask GetReportMask() => stateFlagsMap[currentState].reportMask;
}
