// #define DEBUG_CHARACTER_PHYSICS_NORMALS
// #define DEBUG_CHARACTER_PHYSICS_DELTA_POS
// #define DEBUG_CHARACTER_PHYSICS_BREAK_ON_PENETRATION

using System;
using UnityEngine;

namespace Base {
    public enum WalkType {
        Success,
        Fail,
    }

    public interface ICharacterPhysicsController {
        /* Controlling methods */
        public bool ShouldJump();
        public float GetWalkSpeed();
        public float GetJumpSpeed();

        /* Physics callbacks */
        public void OnWalk(WalkType walkType);
        public void OnSuccessfulJump();
        public void OnGroundChange(Vector2 groundNormal, int offGroundTicks);
    }

// FIXME misbehaves on steep slopes. This might be the fix: https://defold.com/manuals/physics-resolving-collisions/
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterPhysics : MonoBehaviour {
        private const float minMoveDistance = 0.001f;
        private const float shellRadius = 0.01f;
        private const int instantSpeedIterCount = 2;

        // Parameters. Do not change in code
        [SerializeField] private bool doGroundSnapping = true;
        [SerializeField] private float gravityModifier = 1f;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float minGroundNormalY = .65f;
        [SerializeField] private bool persistentSpeedFriction = true;

        // Object state. Reset when needed
        private int ticksOffGround;
        private int ticksSinceLastJump;
        private Vector2 velocity;
        private Vector2 groundNormal;
        private Vector2 oldGroundNormal;

        // Buffers and caches. Do not touch.
        private Rigidbody2D rb;
        private ICharacterPhysicsController controller;
        private ContactFilter2D contactFilter;
        private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            controller = GetComponent<ICharacterPhysicsController>();

            contactFilter.SetLayerMask(collisionMask);
        }

        private void OnEnable() {
            rb.isKinematic = true;
        }

        public void Reset() {
            ticksOffGround = 0;
            ticksSinceLastJump = 0;
            groundNormal = Vector2.up;
            velocity = Vector2.zero;
        }

        private void FixedUpdate() {
            bool isGrounded = IsOnGround();
            int oldTicksOffGround = ticksOffGround;
            oldGroundNormal = groundNormal;
            Vector2 alongGround = -Vector2.Perpendicular(groundNormal);

            ticksOffGround = Math.Min(ticksOffGround + 1, 100);
            groundNormal = Vector2.up;

            ProcessWalking(alongGround * controller.GetWalkSpeed());
            ProcessJump(isGrounded);

            ProcessPersistentSpeed();

            if (doGroundSnapping) {
                GroundSnapping();
            }

#if DEBUG_CHARACTER_PHYSICS_NORMALS
        Vector2 pos = rb.position;
        Color normalCol = IsOnGround() ? Color.green : Color.red;
        Debug.DrawLine(pos,pos + groundNormal, normalCol);
#endif
        }

        /* Physics simulation steps */

        private void ProcessWalking(Vector2 walkSpeed) {
            float walkSpeedIncrement = walkSpeed.magnitude / instantSpeedIterCount;
            Vector2 adjustedWalkSpeed = walkSpeed.normalized * walkSpeedIncrement;

            // TO avoid messaging the controller that we "failed to move"
            if (walkSpeedIncrement < 0.0001) {
                return;
            }

            for (int i = 0; i < instantSpeedIterCount; ++i) {
                if (adjustedWalkSpeed.magnitude * Time.fixedDeltaTime < minMoveDistance) {
                    controller.OnWalk(WalkType.Fail);
                    break;
                }

                Vector2 speed = walkSpeedIncrement * adjustedWalkSpeed.normalized;
                adjustedWalkSpeed = Move(speed, true, false);
            }
        }

        private void ProcessJump(bool isGrounded) {
            ticksSinceLastJump = Math.Min(ticksSinceLastJump + 1, 10);
            if (!isGrounded || !controller.ShouldJump()) {
                return;
            }

            controller.OnSuccessfulJump();
            velocity.y = controller.GetJumpSpeed();
            ticksSinceLastJump = 0;
        }

        private void ProcessPersistentSpeed() {
            Vector2 gravity = Physics2D.gravity * (Time.fixedDeltaTime * gravityModifier);
            velocity = Move(velocity + gravity, true, persistentSpeedFriction);
        }

        private void GroundSnapping() {
            bool isItTimeToSnap = ticksOffGround == 1 && ticksSinceLastJump > 2;
            RaycastHit2D hit = Physics2D.Raycast(
                rb.position,
                Vector2.down,
                2f, contactFilter.layerMask);

            if (!isItTimeToSnap || !hit || !IsGroundNormal(hit.normal)) {
                return;
            }

            SetGroundNormal(hit.normal);
            Move(hit.normal * -8f, false, false);
        }

        public bool IsOnGround() => ticksOffGround == 0;

        /* Private API */

        // TODO this doesn't work well when infinite friction is disabled. I should fix that later
        private Vector2 Move(Vector2 moveVelocity, bool doGroundCheck, bool infiniteGroundFriction) {
            Vector2 deltaPosition = moveVelocity * Time.fixedDeltaTime;
            float distance = deltaPosition.magnitude;

            if (distance <= minMoveDistance) {
                return moveVelocity;
            }

            int collisionCount = rb.Cast(deltaPosition, contactFilter, hitBuffer, distance + shellRadius);

            for (int i = 0; i < collisionCount; ++i) {
                Vector2 normal = hitBuffer[i].normal;
                bool isGroundNormal = doGroundCheck && IsGroundNormal(normal);
                float projection = Vector2.Dot(moveVelocity, normal);

                if (projection < 0f) {
                    moveVelocity -= projection * normal;
                }

                if (isGroundNormal) {
                    SetGroundNormal(normal);
                }

                if (isGroundNormal && infiniteGroundFriction) {
                    moveVelocity = Vector2.zero;
                }

                distance = Mathf.Min(distance, hitBuffer[i].distance - shellRadius);

#if DEBUG_CHARACTER_PHYSICS_BREAK_ON_PENETRATION
            if (distance < 0f) {
                Debug.LogError("Penetration triggered");
                Debug.Break();
            }
#endif
            }

            deltaPosition = deltaPosition.normalized * distance;
            rb.position += deltaPosition;

#if DEBUG_CHARACTER_PHYSICS_DELTA_POS
        Vector2 pos = rb.position;
        Debug.DrawLine(pos - deltaPosition.normalized, pos, Color.yellow);
#endif

            return moveVelocity;
        }

        private bool IsGroundNormal(Vector2 normal) => normal.y > minGroundNormalY;

        private void SetGroundNormal(Vector2 normal) {
            controller.OnGroundChange(normal, ticksOffGround);

            ticksOffGround = 0;
            oldGroundNormal = groundNormal;
            groundNormal = normal;
        }
    }
}
