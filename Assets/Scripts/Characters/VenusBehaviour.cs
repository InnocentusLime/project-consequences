// TODO make breakable pod

using System.Collections.Generic;
using UnityEngine;
using WeaponSys;

namespace Characters {
    public enum VenusState {
        Dormant,
        Active,
        Dead,
    }

    [RequireComponent(typeof(WeaponSys.VenusNom))]
    public class VenusBehaviour : CharacterBehaviour<VenusState> {
        private static readonly Dictionary<VenusState, StateFlags> stateFlagsMapImpl = new(){
            { VenusState.Dormant , new StateFlags {
                attack = false,
                sightMask = 0,
                reportMask = 0,
                physicsType = CharacterPhysicsType.NoPhysics,
            }},
            { VenusState.Active, new StateFlags {
                attack = true,
                sightMask = (1 << 6) | (1 << 9) | (1 << 10), // Must be "Ground | Entity | Player"
                reportMask = (1 << 9) | (1 << 10), // Must be "Entity | Player"
                physicsType = CharacterPhysicsType.NoPhysics,
            }},
            { VenusState.Dead, new StateFlags {
                attack = false,
                sightMask = 0,
                reportMask = 0,
                physicsType = CharacterPhysicsType.NoPhysics,
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
}
