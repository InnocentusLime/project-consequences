// #define CHARACTER_BEHAVIOUR_DEBUG_STATE_TRANSITIONS

using System;
using System.Collections.Generic;
using Base;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSys;

namespace Characters {
    public enum CharacterPhysicsType {
        NoPhysics,
        UnityPhysics,
        CharacterPhysics,
    }

    [Serializable]
    public struct StateFlags {
        public bool attack;
        public LayerMask sightMask;
        public LayerMask reportMask;
        public CharacterPhysicsType physicsType;
    }

    public interface IWeapon {
        // Do an attack at angle `angle`.
        // The return value indicates if the attack has been successful or not.
        public bool Attack(float angle) => false;

        // Do an attack right at `obj`.
        // The return value indicates if the attack has been successful or not.
        public bool Attack(GameObject obj) => false;
    }

    [RequireComponent(typeof(Eyesight), typeof(CharacterPhysics))]
    public abstract class CharacterBehaviour<T> : CursedBehaviour, IDamageable, IEyesightClient,
        ICharacterPhysicsController {
        // Components
        private IWeapon weapon;
        private Eyesight eyesight;
        private CharacterPhysics characterPhysics;
        private Rigidbody2D rBody2D;

        // Private state fields
        protected T currentState { get; private set;  }

        // Interface
        [SerializeField] protected bool hasWeapon;
        private IEyesightClient eyesightClientImplementation;

        /* Required methods */

        protected virtual Dictionary<T, StateFlags> stateFlagsMap => null;
        protected abstract T DefaultState();

        /* Methods for overloading */

        public virtual bool ShouldJump() => false;
        public virtual float GetWalkSpeed() => 0f;
        public virtual float GetJumpSpeed() => 0f;

        /* Event handlers */

        public virtual void Damage(DamageType damageType) {}
        public virtual void OnSeenObject(GameObject obj) {}
        public virtual void OnWalk(WalkType walkType) {}
        public virtual void OnSuccessfulJump() {}
        public virtual void OnGroundChange(Vector2 groundNormal, int offGroundTicks) {}

        /* Child API */
        protected override void Awake() {
            base.Awake();

            weapon = GetComponent<IWeapon>();
            eyesight = GetComponent<Eyesight>();
            characterPhysics = GetComponent<CharacterPhysics>();
            rBody2D = GetComponent<Rigidbody2D>();

            SetStateInner(DefaultState(), true);
        }

        public void LookInDirection(Vector2 direction) {
            eyesight.sightDirection = direction;
        }

        public Vector2 GetEyeSightDirection() {
            return eyesight.sightDirection;
        }

        public void SetState(T newState) {
            if (currentState.Equals(newState)) {
                return;
            }

            SetStateInner(newState);
        }

        public bool Attack() {
            float angle = Vector2.SignedAngle(Vector2.right, eyesight.sightDirection);
            return hasWeapon && weapon.Attack(angle);
        }

        public bool Attack(GameObject obj) => hasWeapon && weapon.Attack(obj);

        /* Private API */

        private void SetStateInner(T newState, bool init = false) {
#if CHARACTER_BEHAVIOUR_DEBUG_STATE_TRANSITIONS
        Debug.Log(this.GetType().Name + ": " +
            currentState + " -> " + newState + "\nnew flags:\n" +
            JsonUtility.ToJson(stateFlagsMap[newState], true));
#endif

            T oldState = currentState;
            currentState = newState;
            ApplyFlags(stateFlagsMap[currentState], stateFlagsMap[oldState], init);
        }

        private void ApplyFlags(StateFlags newFlags, StateFlags oldFlags, bool init) {
            // Gun
            hasWeapon = newFlags.attack;

            // Physics
            if (newFlags.physicsType != oldFlags.physicsType || init) {
                // Cleanup old physics
                switch (oldFlags.physicsType) {
                    case CharacterPhysicsType.NoPhysics:
                        break;
                    case CharacterPhysicsType.UnityPhysics:
                        rBody2D.velocity = Vector2.zero;
                        break;
                    case CharacterPhysicsType.CharacterPhysics:
                        characterPhysics.enabled = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                // Init new physics
                switch (newFlags.physicsType) {
                    case CharacterPhysicsType.NoPhysics:
                        rBody2D.simulated = false;
                        rBody2D.bodyType = RigidbodyType2D.Static;
                        break;
                    case CharacterPhysicsType.UnityPhysics:
                        rBody2D.simulated = true;
                        rBody2D.bodyType = RigidbodyType2D.Dynamic;
                        rBody2D.velocity = characterPhysics.lastVelocity;
                        break;
                    case CharacterPhysicsType.CharacterPhysics:
                        rBody2D.simulated = true;
                        characterPhysics.enabled = true;
                        rBody2D.bodyType = RigidbodyType2D.Kinematic;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public LayerMask GetSightMask() => stateFlagsMap[currentState].sightMask;

        public LayerMask GetReportMask() => stateFlagsMap[currentState].reportMask;
    }
}
