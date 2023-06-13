using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour {
    public float speed = 8f;
    public float jump = 8f;
    public float gravity = 8f;

    private float MoveHor;
    private float MoveVer;

    public Rigidbody2D rb;

    void Update() {
        MoveHor = Input.GetAxisRaw("Horizontal");
        Vector2 newRb = new Vector2(MoveHor * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump")) {
            newRb.y += jump;
        }

        newRb.y -= gravity * Time.deltaTime;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 2), 0, newRb * Time.deltaTime,
            newRb.magnitude * Time.deltaTime);



        rb.velocity = newRb;
    }
}
