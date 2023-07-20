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

[RequireComponent(typeof(EyeSight), typeof(CharacterPhysics))]
public abstract class CharacterBehaviour<T> : CursedBehaviour, IDamageable, IEyesightClient {
    // Components
    private Gun gun;
    private CharacterPhysics physics;

    // Private state fields
    private T currentState;
    protected static readonly Dictionary<T, StateFlags> stateFlagsMap = null;

    // Interface
    [SerializeField] protected bool hasGun;
    private IEyesightClient eyesightClientImplementation;

    /* Required methods */

    protected abstract T DefaultState();

    /* Event handlers */

    public abstract void Damage(DamageType damageType);
    public abstract void OnSeenObject(GameObject obj);

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
