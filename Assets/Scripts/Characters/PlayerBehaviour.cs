using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using WeaponSys;

namespace Characters {
    public enum PlayerState {
        Normal,
        Dead,
        Haunted,
    }

    [Serializable]
    public class PlayerShootEvent : UnityEvent<Vector2, Vector2> {
    }

    [RequireComponent(typeof(Interaction))]
    public class PlayerBehaviour : CharacterBehaviour<PlayerState>, IEdible {
        private static readonly Dictionary<PlayerState, StateFlags> stateFlagsMapImpl = new() {
            {
                PlayerState.Normal, new StateFlags {
                    attack = true,
                    sightMask = 0,
                    reportMask = 0,
                    physics = true,
                    unitySimulate = true,
                }
            }, {
                PlayerState.Haunted, new StateFlags {
                    attack = false,
                    sightMask = 0,
                    reportMask = 0,
                    physics = true,
                    unitySimulate = true,
                }
            }, {
                PlayerState.Dead, new StateFlags {
                    attack = false,
                    sightMask = 0,
                    reportMask = 0,
                    physics = false,
                    unitySimulate = false,
                }
            },
        };

        protected override Dictionary<PlayerState, StateFlags> stateFlagsMap => stateFlagsMapImpl;

        private Interaction interaction;
        private Camera mainCamera;

        [SerializeField] private float horizontalSpeed;
        [SerializeField] private float jumpTakeoffSpeed;
        [SerializeField] private PlayerShootEvent playerShootEvent;

        protected override PlayerState DefaultState() => PlayerState.Normal;

        public override bool ShouldJump() => Input.GetKey(KeyCode.Space);

        public override float GetWalkSpeed() => horizontalSpeed * Input.GetAxisRaw("Horizontal");

        public override float GetJumpSpeed() => jumpTakeoffSpeed;

        public override void Damage(DamageType damageType) {
            SetState(PlayerState.Dead);
        }

        public FoodType GetFoodType() => FoodType.Harmless;

        protected override void Awake() {
            base.Awake();

            interaction = GetComponent<Interaction>();

            mainCamera = Camera.main;
            Assert.IsNotNull(mainCamera);
        }

        private void Update() {
            float walkSpeed = GetWalkSpeed();

            if (walkSpeed != 0) {
                LookInDirection(new Vector2(walkSpeed, 0));
            }

            Vector2 lookDirection = GetEyeSightDirection();

            if (Input.GetMouseButtonDown(0)) {
                if (Attack()) {
                    playerShootEvent.Invoke(lookDirection, transform.position);
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                interaction.Interact();
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                SetState(PlayerState.Dead);
            }
        }

        protected override void OnConsequenceTime() {
            SetState(PlayerState.Haunted);
        }
    }
}
