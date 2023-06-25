using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;

    void Start()
    {
        Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();

        float angle = rigidBody2D.rotation * Mathf.Deg2Rad;
        rigidBody2D.velocity = new Vector2((float)(moveSpeed * Math.Cos(Mathf.PI / 2 + angle)),
            (float)(moveSpeed * Math.Sin(Mathf.PI / 2 + angle)));

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Hittable hittable = col.gameObject.GetComponent<Hittable>();

        if (hittable != null) {
            hittable.bulletHitEvent.Invoke(gameObject);
        }

        Destroy(gameObject);
    }
}
