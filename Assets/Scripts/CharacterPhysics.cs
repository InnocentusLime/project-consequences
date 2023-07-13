#define DEBUG_CHARACTER_PHYSICS_NORMALS
#define DEBUG_CHARACTER_PHYSICS_DELTA_POS

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterPhysics : MonoBehaviour {
    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;
    private const int instantSpeedIterCount = 2;

    [SerializeField] private bool doGroundSnapping = true;
    [SerializeField] private float gravityModifier = 1f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float minGroundNormalY = .65f;

    private int ticksOffGround;
    private int ticksJump;
    private bool jumpQueued;
    private float targetJumpSpeed;
    private Vector2 velocity;
    private float targetWalkSpeed;
    private Vector2 groundNormal;

    private Rigidbody2D rb;
    private ContactFilter2D contactFilter;
    private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    public void Reset() {
        ticksOffGround = 0;
        ticksJump = 0;
        groundNormal = Vector2.up;
        targetJumpSpeed = 0f;
        targetWalkSpeed = 0f;
        jumpQueued = false;
        velocity = Vector2.zero;
    }

    public void MoveHorizontally(float hSpeed) {
        targetWalkSpeed = hSpeed;
    }

    // TODO coyote time?
    public void Jump(float jumpSpeed) {
        if (ticksOffGround > 0) {
            return;
        }

        jumpQueued = true;
        targetJumpSpeed = jumpSpeed;
    }

    private void Awake() {
        targetWalkSpeed = 1f;
        rb = GetComponent<Rigidbody2D>();

        contactFilter.useLayerMask = true;
        contactFilter.layerMask = collisionMask;
    }

    private void FixedUpdate() {
        Vector2 alongGround = -Vector2.Perpendicular(groundNormal);

        velocity += Physics2D.gravity * (gravityModifier * Time.fixedDeltaTime);
        ticksOffGround = Math.Min(ticksOffGround + 1, 100);
        groundNormal = Vector2.up;

        // For instant velocities (AKA velocities that make no sense)
        float walkSpeedIncrement = Mathf.Abs(targetWalkSpeed) / instantSpeedIterCount;
        Vector2 instantMoveDirection = alongGround * Mathf.Sign(targetWalkSpeed);
        for (int i = 0; i < instantSpeedIterCount; ++i) {
            Vector2 newMoveDirection = Move(
                walkSpeedIncrement * instantMoveDirection,
                true,
                false);
            if (newMoveDirection.magnitude * Time.fixedDeltaTime < minMoveDistance) {
                break;
            }

            instantMoveDirection = newMoveDirection.normalized;
        }

        ticksJump = Math.Min(ticksJump + 1, 10);
        if (jumpQueued) {
            velocity.y = targetJumpSpeed;
            jumpQueued = false;
            targetJumpSpeed = 0f;
            ticksJump = 0;
        }

        // For "persistent" velocities (AKA velocities that make sense from physical point of view)
        velocity = Move(velocity, true, true);

        if (ShouldSnapToGround()) {
            // Well, more like "rush to ground"
            SnapToGround();
        }

#if DEBUG_CHARACTER_PHYSICS_NORMALS
        Debug.DrawLine(
            rb.position,
            rb.position + groundNormal * 2f,
            ticksOffGround == 0 ? Color.green : Color.red);
#endif
    }

    private bool ShouldSnapToGround() => doGroundSnapping && ticksOffGround == 1 && ticksJump > 2;

    private void SnapToGround() {
        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            Vector2.down,
            2f,
            contactFilter.layerMask);

        if (!hit) {
            return;
        }

        Vector2 normal = hit.normal;
        if (!GroundCheck(normal)) {
            return;
        }

        Move(normal * -8f, false, false);
    }

    private Vector2 Move(
        Vector2 moveVelocity,
        bool doGroundCheck,
        bool infiniteGroundFriction) {
        Vector2 deltaPosition = moveVelocity * Time.fixedDeltaTime;
        float distance = deltaPosition.magnitude;

        if (distance <= minMoveDistance) {
            return moveVelocity;
        }

        int collisionCount = rb.Cast(
            deltaPosition,
            contactFilter,
            hitBuffer,
            distance + shellRadius);
        for (int i = 0; i < collisionCount; ++i) {
            Vector2 normal = hitBuffer[i].normal;
            bool isGroundNormal = doGroundCheck && GroundCheck(normal);
            float projection = Vector2.Dot(moveVelocity, normal);

            if (projection < 0f) {
                moveVelocity -= projection * normal;
            }

            if (isGroundNormal && infiniteGroundFriction) {
                moveVelocity = Vector2.zero;
            }

            distance = Mathf.Min(distance, hitBuffer[i].distance - shellRadius);
        }

#if DEBUG_CHARACTER_PHYSICS_DELTA_POS
        Debug.DrawLine(
            rb.position,
            rb.position + (deltaPosition.normalized * distance).normalized,
            Color.yellow);
#endif

        rb.position += deltaPosition.normalized * distance;

        return moveVelocity;
    }

    private bool GroundCheck(Vector2 normal) {
        if (normal.y <= minGroundNormalY) {
            return false;
        }

        ticksOffGround = 0;
        groundNormal = normal;

        return true;
    }
}
