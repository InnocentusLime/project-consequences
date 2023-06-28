using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ZombieBehaviour : CursedBehaviour {
    public float moveSpeed;
    public float angryMul;
    public float seeDistance;

    private bool wasSeen = false;
    private bool isDead = false;
    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start() {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (isDead) {
            return;
        }

        RaycastHit2D hitGround = Physics2D.Raycast(transform.position,
            new Vector2(rigidBody2D.velocity.x, 0),
            distance: transform.localScale.x / 2 + 0.1f,
            layerMask: LayerMask.GetMask("Ground"));

        RaycastHit2D hitPlayer = Physics2D.Raycast(
            transform.position +
            new Vector3(transform.localScale.x / 2 * Mathf.Sign(moveSpeed) + 0.05f, 0, 0),
            new Vector2(rigidBody2D.velocity.x, 0),
            distance: 0.1f,
            layerMask: LayerMask.GetMask("Entities"));

        RaycastHit2D seePlayer = Physics2D.Raycast(
            transform.position +
            new Vector3(transform.localScale.x / 2 * Mathf.Sign(moveSpeed) + 0.05f, 0, 0),
            new Vector2(rigidBody2D.velocity.x, 0),
            distance: seeDistance,
            layerMask: LayerMask.GetMask("Entities"));

        if (hitGround.collider != null) {
            moveSpeed = -moveSpeed;
        }

        if (hitPlayer.collider != null && hitPlayer.collider.gameObject == GlobalRoomState.player) {
            Destroy(GlobalRoomState.player);
        }

        if (seePlayer.collider != null && seePlayer.collider.gameObject == GlobalRoomState.player) {
            if (!wasSeen) {
                wasSeen = true;
                moveSpeed *= angryMul;
            }

            GetComponent<SpriteRenderer>().color = new Color(100.0f / 255, 147.0f / 255, 27.0f / 255);
        }

        if (wasSeen) {
            float sign = Mathf.Sign(GlobalRoomState.player.transform.position.x - transform.position.x);

            rigidBody2D.velocity = new Vector2(sign * Mathf.Abs(moveSpeed), rigidBody2D.velocity.y);
        } else {
            rigidBody2D.velocity = new Vector2(moveSpeed, rigidBody2D.velocity.y);
        }
    }

    public void OnBulletHit(GameObject bullet) {
        isDead = true;
        wasSeen = false;
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
    }

    protected override void OnConsequenceTime() {
        if (!isDead) {
            Destroy(gameObject);
            return;
        }

        isDead = false;
        GetComponent<SpriteRenderer>().color = new Color(0f, 147.0f / 255, 27.0f / 255);
    }

    protected override void OnMadnessChange(int madnessLevel) {
    }
}
