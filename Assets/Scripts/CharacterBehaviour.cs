using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StateFlags {
    public bool gun;
    public bool sight;
    public bool physics;
}

[RequireComponent(typeof(EyeSight), typeof(CharacterPhysics),
    typeof(Gun))]
[RequireComponent(typeof(Damageable))]
public abstract class CharacterBehaviour<T> : CursedBehaviour {
    // Components
    private Gun gun;
    private EyeSight eyeSight;
    private CharacterPhysics physics;
    private Damageable damageable;

    // Private state fields
    private T currentState;
    protected static readonly Dictionary<T, StateFlags> stateFlagsMap = null;

    // Interface
    [SerializeField] protected bool hasGun;

    /* Required methods */

    protected abstract T DefaultState();

    /* Event handlers */

    protected abstract void OnSeenObject(GameObject obj);
    protected abstract void OnDamage(DamageType damageType);

    /* Child API */

    protected override void ExtraAwake() {
        base.ExtraAwake();

        gun = GetComponent<Gun>();
        eyeSight = GetComponent<EyeSight>();
        physics = GetComponent<CharacterPhysics>();
        damageable = GetComponent<Damageable>();

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
    protected void Damage(DamageType damageType) => damageable.Damage(damageType);
    protected void SetVisionTarget(LayerMask mask) => eyeSight.reportMask = mask;
    protected void SetVisionBlocker(LayerMask mask) => eyeSight.sightMask = mask;

    /* Private API */

    private void ApplyFlags(StateFlags flags) {
        // Gun
        hasGun = flags.gun;
        gun.enabled = flags.gun;

        // Eyesight
        eyeSight.enabled = flags.sight;

        // Physics
        physics.enabled = flags.physics;
    }
}
