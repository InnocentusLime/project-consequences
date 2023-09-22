using System;
using System.Collections.Generic;
using UnityEngine;
using WeaponSys;

namespace Characters {
    public enum ShadowState {
        Dormant,
        Awakened,
        Ascended,
    }

// TODO give shotgun
    [RequireComponent(typeof(Gun))]
    public class ShadowBehaviour : CharacterBehaviour<ShadowState> {
        private static readonly Dictionary<ShadowState, StateFlags> stateFlagsMapImpl = new(){
            { ShadowState.Dormant , new StateFlags {
                attack = false,
                sightMask = 0,
                reportMask = 0,
                physics = false,
            }},
            { ShadowState.Awakened , new StateFlags {
                attack = true,
                sightMask = (1 << 6) | (1 << 10 ), // Must be "Ground | Player"
                reportMask = 1 << 10, // Must be "Player"
                physics = false,
            }},
            { ShadowState.Ascended , new StateFlags {
                attack = false,
                sightMask = 0, // Must be "Ground | Player"
                reportMask = 0, // Must be "Player"
                physics = false,
            }},
        };

        protected override Dictionary<ShadowState, StateFlags> stateFlagsMap => stateFlagsMapImpl;
        private SpriteRenderer spriteRenderer;

        protected override void Awake() {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            spriteRenderer.color = currentState switch {
                ShadowState.Dormant => Color.clear,
                ShadowState.Awakened => new Color(0.525f, 0.01f, 0.525f),
                ShadowState.Ascended => Color.black,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void OnConsequenceTime() {
            SetState(ShadowState.Awakened);
        }

        protected override ShadowState DefaultState() => ShadowState.Dormant;

        public override void OnSeenObject(GameObject obj) {
            Attack();
            SetState(ShadowState.Ascended);
        }
    }
}
