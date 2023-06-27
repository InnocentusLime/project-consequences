using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code from: https://github.com/Vidsneezes/2dKinematicControllerUnity

public class PlayerMovement : MonoBehaviour {
    public bool isRightPressed;
    public bool isLeftPressed;
    public bool wasJumpPressed;

    public float moveSpeed;
    public float gravity;
    public float startJumpSpeed;
    public LayerMask groundMask;
    [HideInInspector] public BoxCollider2D hitBox;
    [HideInInspector] public SpriteRenderer mainSprite;
    public bool onGround;

    public Vector2 velocity;
    private Vector2 direction;
    [HideInInspector] public Rigidbody2D rBody2d;
    private float actualMoveSpeed;
    [HideInInspector] public GameObject playerSpawnPoint;

    private void Awake() {
        rBody2d = GetComponent<Rigidbody2D>();
        mainSprite = GetComponent<SpriteRenderer>();
        hitBox = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    private void Start() {
        direction = Vector2.right;
    }

    // Update is called once per frame
    private void Update() {
        isRightPressed = Input.GetKey(KeyCode.D);
        isLeftPressed = Input.GetKey(KeyCode.A);
        wasJumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (isRightPressed) {
            direction.x = 1;
            mainSprite.flipX = false;
            actualMoveSpeed = moveSpeed;
        } else if (isLeftPressed) {
            direction.x = -1;
            mainSprite.flipX = true;
            actualMoveSpeed = moveSpeed;
        } else {
            actualMoveSpeed = 0;
        }

        if (wasJumpPressed && onGround) {
            velocity.y = startJumpSpeed;
            onGround = false;
        }
    }

    private void FixedUpdate() {
        Vector2 currentPosition = rBody2d.position;
        velocity.y -= gravity * Time.deltaTime;

        //horizontal check
        velocity.x = direction.x * actualMoveSpeed;
        float finalHorVelocity = velocity.x;

        if (PlaceMeet(currentPosition.x + finalHorVelocity * Time.deltaTime, currentPosition.y, groundMask)) {
            while (!PlaceMeet(currentPosition.x + ((finalHorVelocity * Time.deltaTime) / 8), currentPosition.y,
                       groundMask)) {
                currentPosition.x += ((finalHorVelocity * Time.deltaTime) / 8);
            }

            finalHorVelocity = 0;
        }

        currentPosition.x += finalHorVelocity * Time.deltaTime;

        //vertical check
        float finalVerVelocity = velocity.y;
        if (PlaceMeet(currentPosition.x, currentPosition.y + finalVerVelocity * Time.deltaTime, groundMask)) {
            while (!PlaceMeet(currentPosition.x, currentPosition.y + ((finalVerVelocity * Time.deltaTime) / 8),
                       groundMask)) {
                currentPosition.y += ((finalVerVelocity * Time.deltaTime) / 8);
            }

            finalVerVelocity = 0;
        }

        currentPosition.y += finalVerVelocity * Time.deltaTime;

        rBody2d.MovePosition(currentPosition);

        if (PlaceMeet(currentPosition.x, currentPosition.y + (1f / 8f), groundMask)) {
            velocity.y = 0;
        }

        if (PlaceMeet(currentPosition.x, currentPosition.y - (1f / 8f), groundMask)) {
            velocity.y = 0;
            onGround = true;
        } else {
            onGround = false;
        }
    }

    private bool PlaceMeet(float xPos, float yPos, LayerMask mask) {
        Vector2 position = new Vector2(xPos + hitBox.offset.x, yPos + hitBox.offset.y);
        Collider2D[] collider2d = Physics2D.OverlapBoxAll(position, hitBox.size, 0, mask);
        return collider2d.Length > 0;
    }
}
