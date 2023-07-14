// #define DEBUG_CHARACTER_PHYSICS_NORMALS
// #define DEBUG_CHARACTER_PHYSICS_DELTA_POS

using System;
using UnityEngine;

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
    private bool jumpQueued;
    private float targetJumpSpeed;
    private Vector2 velocity;
    private float targetWalkSpeed;
    private Vector2 groundNormal;

    // Buffers and caches. Do not touch.
    private Rigidbody2D rb;
    private ContactFilter2D contactFilter;
    private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        contactFilter.SetLayerMask(collisionMask);
    }

    public void Reset() {
        ticksOffGround = 0;
        ticksSinceLastJump = 0;
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
        if (!IsOnGround()) {
            return;
        }

        jumpQueued = true;
        targetJumpSpeed = jumpSpeed;
    }

    private void FixedUpdate() {
        Vector2 alongGround = -Vector2.Perpendicular(groundNormal);

        ticksOffGround = Math.Min(ticksOffGround + 1, 100);
        groundNormal = Vector2.up;

        ProcessInstantSpeed(alongGround * targetWalkSpeed);
        ProcessJump();

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

    private void ProcessInstantSpeed(Vector2 instantSpeed) {
        float walkSpeedIncrement = instantSpeed.magnitude / instantSpeedIterCount;
        Vector2 instantMoveDirection = instantSpeed.normalized;

        for (int i = 0; i < instantSpeedIterCount; ++i) {
            Vector2 moveSpeed = walkSpeedIncrement * instantMoveDirection;
            Vector2 adjustedMoveSpeed = Move(moveSpeed, true, false);

            if (adjustedMoveSpeed.magnitude * Time.fixedDeltaTime < minMoveDistance) {
                break;
            }

            instantMoveDirection = adjustedMoveSpeed.normalized;
        }
    }

    private void ProcessJump() {
        ticksSinceLastJump = Math.Min(ticksSinceLastJump + 1, 10);
        if (!jumpQueued) {
            return;
        }

        velocity.y = targetJumpSpeed;
        jumpQueued = false;
        targetJumpSpeed = 0f;
        ticksSinceLastJump = 0;
    }

    private void ProcessPersistentSpeed() {
        Vector2 inputVelocity = velocity + Physics2D.gravity * (gravityModifier * Time.fixedDeltaTime);
        velocity = Move(inputVelocity, true, persistentSpeedFriction);
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
        }

        deltaPosition = deltaPosition.normalized * distance;
        rb.position += deltaPosition;

#if DEBUG_CHARACTER_PHYSICS_DELTA_POS
        Vector2 pos = rb.position;
        Debug.DrawLine(pos - deltaPosition, pos, Color.yellow);
#endif

        return moveVelocity;
    }

    public bool IsOnGround() => ticksOffGround == 0;

    private bool IsGroundNormal(Vector2 normal) => normal.y > minGroundNormalY;

    private void SetGroundNormal(Vector2 normal) {
        ticksOffGround = 0;
        groundNormal = normal;
    }
}
