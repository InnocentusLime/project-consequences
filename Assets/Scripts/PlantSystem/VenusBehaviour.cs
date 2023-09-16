// TODO make breakable pod

using System;
using System.Collections.Generic;
using UnityEngine;

public enum VenusState {
    Dormant,
    Active,
    Dead,
}

[RequireComponent(typeof(VenusNom))]
public class VenusBehaviour : CharacterBehaviour<VenusState> {
    private static readonly Dictionary<VenusState, StateFlags> stateFlagsMapImpl = new(){
        { VenusState.Dormant , new StateFlags {
            attack = false,
            sightMask = 0,
            reportMask = 0,
            physics = false,
        }},
        { VenusState.Active, new StateFlags {
            attack = true,
            sightMask = (1 << 6) | (1 << 9) | (1 << 10), // Must be "Ground | Entity | Player"
            reportMask = (1 << 9) | (1 << 10), // Must be "Entity | Player"
            physics = false,
        }},
        { VenusState.Dead, new StateFlags {
            attack = false,
            sightMask = 0,
            reportMask = 0,
            physics = false,
        }},
    };

    protected override Dictionary<VenusState, StateFlags> stateFlagsMap => stateFlagsMapImpl;

    [SerializeField] private GameObject display;

    private void Update() {
        display.SetActive(currentState == VenusState.Active);
    }

    protected override void OnConsequenceTime() {
        SetState(VenusState.Active);
    }

    protected override VenusState DefaultState() => VenusState.Dormant;

    public override void Damage(DamageType damageType) {
        SetState(VenusState.Dead);
    }

    public override void OnSeenObject(GameObject obj) {
        Attack(obj);
    }
}
