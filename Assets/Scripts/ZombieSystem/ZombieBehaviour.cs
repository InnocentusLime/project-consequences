using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ZombieBehaviour : CursedBehaviour {
    private Damageable damageable;

    public Vector2 movingDirection;
    private EyeSight eyeSight;

    public float moveSpeed;
    public float angryMul;

    private bool wasSeen = false;
    private bool isDead = false;
    private Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    protected override void OnMadnessChange(int madnessLevel) {
    }

    protected override void ExtraAwake() {
        damageable = GetComponent<Damageable>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        eyeSight = GetComponent<EyeSight>();
        movingDirection = eyeSight.sightDirection;
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
            new Vector3((transform.localScale.x / 2 + 0.5f) * Mathf.Sign(movingDirection.x), 0, 0),
            new Vector2(rigidBody2D.velocity.x, 0),
            distance: 0.1f,
            layerMask: LayerMask.GetMask("Player"));

        if (hitPlayer.collider != null && hitPlayer.collider.gameObject == GlobalRoomState.player) {
            GlobalRoomState.player.GetComponent<Damageable>().Damage(DamageType.ZombiePunch);
        }

        if (hitGround.collider != null) {
            movingDirection = -movingDirection;
            eyeSight.sightDirection = movingDirection;
            eyeSight.rayOffset = -eyeSight.rayOffset;
        }


        if (wasSeen) {
            movingDirection =
                new Vector2(Mathf.Sign(GlobalRoomState.player.transform.position.x - transform.position.x),
                              movingDirection.y);

            eyeSight.sightDirection = movingDirection;

            if (Mathf.Sign(eyeSight.sightDirection.x) != Mathf.Sign(eyeSight.rayOffset.x)) {
                eyeSight.rayOffset = -eyeSight.rayOffset;
            }
        }

        rigidBody2D.velocity = new Vector2(movingDirection.x * moveSpeed, movingDirection.y + rigidBody2D.velocity.y);
    }

    public void OnBulletHit(GameObject bullet) {
        isDead = true;
        wasSeen = false;
        eyeSight.enabled = !eyeSight.enabled;
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
    }

    public void OnSeeingObject(GameObject seeingObject) {
        if (!wasSeen) {
            wasSeen = true;
            moveSpeed *= angryMul;
        }

        GetComponent<SpriteRenderer>().color = new Color(100.0f / 255, 147.0f / 255, 27.0f / 255);
    }

    protected override void OnConsequenceTime() {
        if (!isDead) {
            damageable.Damage(DamageType.DecayDamage);
            return;
        }

        isDead = false;
        moveSpeed *= angryMul;
        GetComponent<SpriteRenderer>().color = new Color(0f, 147.0f / 255, 27.0f / 255);
    }
}
