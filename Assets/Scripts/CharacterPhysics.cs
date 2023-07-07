using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterPhysics : MonoBehaviour {
    private const float minMoveDistance = 0.001f;
    private const float shellRadius = 0.01f;

    public bool isGrounded { get; private set; }
    [SerializeField] private float gravityModifier = 1f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float minGroundNormalY = .65f;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private float targetHSpeed;
    private ContactFilter2D contactFilter;

    private Vector2 groundNormal;
    private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    public void MoveHorizontally(float hSpeed) {
        targetHSpeed = hSpeed;
    }

    public void Jump(float jumpSpeed) {
        if (!isGrounded) {
            return;
        }

        velocity.y = jumpSpeed;
    }

    public void SlowdownJump() {
        if (velocity.y <= 0f) {
            return;
        }

        velocity.y *= .5f;
    }

    private void Awake() {
        targetHSpeed = 1f;
        rb = GetComponent<Rigidbody2D>();

        contactFilter.useLayerMask = true;
        contactFilter.layerMask = collisionMask;
    }

    private void FixedUpdate() {
        velocity += Physics2D.gravity * (gravityModifier * Time.fixedDeltaTime);
        velocity.x = targetHSpeed;

        isGrounded = false;

        Vector2 moveAlongGround = -Vector2.Perpendicular(groundNormal);
        Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

        Move(deltaPosition.x * moveAlongGround, false);
        Move(deltaPosition.y * Vector2.up, true);
    }

    private void Move(Vector2 deltaPosition, bool computingYMovement) {
        float distance = deltaPosition.magnitude;
        if (distance <= minMoveDistance) {
            return;
        }

        int collisionCount = rb.Cast(
            deltaPosition,
            contactFilter,
            hitBuffer,
            distance + shellRadius);
        for (int i = 0; i < collisionCount; ++i) {
            Vector2 normal = hitBuffer[i].normal;

            bool isGroundNormal = GroundCheck(normal, computingYMovement);
            SurfaceReaction(normal, computingYMovement, isGroundNormal);

            distance = Mathf.Min(distance, hitBuffer[i].distance - shellRadius);
        }

        rb.position += deltaPosition.normalized * distance;
    }

    private bool GroundCheck(Vector2 normal, bool computingYMovement) {
        bool isGroundNormal = normal.y > minGroundNormalY;

        isGrounded |= isGroundNormal;
        if (computingYMovement && isGroundNormal) {
            groundNormal = normal;
        }

        return isGroundNormal;
    }

    private void SurfaceReaction(Vector2 normal, bool computingYMovement, bool isGroundNormal) {
        if (computingYMovement && isGroundNormal) {
            normal.x = 0f;
        }

        float projection = Vector2.Dot(velocity, normal);
        if (projection < 0f) {
            velocity -= projection * normal;
        }
    }
}
