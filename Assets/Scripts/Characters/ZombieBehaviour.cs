using System;
using System.Collections.Generic;
using Base;
using Extensions;
using RoomSys;
using UnityEngine;
using WeaponSys;

namespace Characters {
    public enum ZombieState {
        Normal,
        Resurrected,
        Dead,
        Angered,
    }

    [RequireComponent(typeof(ZombieFist))]
    public class ZombieBehaviour : CharacterBehaviour<ZombieState>, IEdible {
        private static readonly Dictionary<ZombieState, StateFlags> stateFlagsMapImpl = new(){
            { ZombieState.Normal, new StateFlags {
                attack = false,
                sightMask = (1 << 6) | (1 << 10 ), // Must be "Ground | Player"
                reportMask = 1 << 10, // Must be "Player"
                physicsType = CharacterPhysicsType.CharacterPhysics,
            }},
            { ZombieState.Resurrected, new StateFlags {
                attack = true,
                sightMask = 0,
                reportMask = 0,
                physicsType = CharacterPhysicsType.CharacterPhysics,
            }},
            { ZombieState.Dead, new StateFlags {
                attack = false,
                sightMask = 0,
                reportMask = 0,
                physicsType = CharacterPhysicsType.UnityPhysics,
            }},
            { ZombieState.Angered, new StateFlags {
                attack = true,
                sightMask = 0,
                reportMask = 0,
                physicsType = CharacterPhysicsType.CharacterPhysics,
            }},
        };

        protected override Dictionary<ZombieState, StateFlags> stateFlagsMap => stateFlagsMapImpl;

        [SerializeField] private bool isWalkingLeft = false;
        private SpriteRenderer spriteRenderer;

        protected override void Awake() {
            base.Awake();

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            spriteRenderer.color = currentState switch {
                ZombieState.Normal => new Color(0f, 147.0f / 255, 27.0f / 255),
                ZombieState.Resurrected => new Color(0f, 147.0f / 255, 27.0f / 255),
                ZombieState.Dead => new Color(0f, 0f, 0f),
                ZombieState.Angered => new Color(100.0f / 255, 147.0f / 255, 27.0f / 255),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void FixedUpdate() {
            if (currentState == ZombieState.Angered) {
                isWalkingLeft = (GlobalRoomState.player.transform.position - transform.position).x < 0f;
            }

            LookInDirection(Vector2.right * (isWalkingLeft ? -1f : 1f));
            if (currentState is ZombieState.Angered or ZombieState.Resurrected) {
                Attack();
            }
        }

        protected override void OnConsequenceTime() {
            ZombieState newState = currentState switch {
                ZombieState.Normal => ZombieState.Dead,
                ZombieState.Resurrected => throw new ArgumentOutOfRangeException(),
                ZombieState.Dead => ZombieState.Resurrected,
                ZombieState.Angered => ZombieState.Dead,
                _ => throw new ArgumentOutOfRangeException(),
            };

            SetState(newState);
        }

        protected override ZombieState DefaultState() => ZombieState.Normal;

        public override float GetWalkSpeed() => currentState switch {
            ZombieState.Normal => 1f,
            ZombieState.Resurrected => 8f,
            ZombieState.Dead => 0f,
            ZombieState.Angered => 3f,
            _ => throw new ArgumentOutOfRangeException()
        } * (isWalkingLeft ? -1f : 1f);

        public override void OnWalk(WalkType walkType) {
            switch (walkType) {
                case WalkType.Success:
                    break;
                case WalkType.Fail:
                    if (currentState != ZombieState.Angered) {
                        isWalkingLeft = !isWalkingLeft;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Damage(DamageType damageType) {
            SetState(ZombieState.Dead);

            // TODO go to a different state
            if (damageType == DamageType.VenusEat) {
                Destroy(gameObject);
            }
        }

        public FoodType GetFoodType() => FoodType.Poisonous;

        public override void OnSeenObject(GameObject obj) {
            SetState(ZombieState.Angered);
        }
    }
}
