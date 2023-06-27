using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float moveSpeed;
    public float lifeTime;
    public GameObject creator;

    private void Start() {
        Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();

        float angle = (rigidBody2D.rotation + 90) * Mathf.Deg2Rad;
        rigidBody2D.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * moveSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        GameObject colGameObject = col.gameObject;

        if (colGameObject == creator) {
            return;
        }

        Hittable hittable = colGameObject.GetComponent<Hittable>();

        if (hittable != null) {
            hittable.bulletHitEvent.Invoke(gameObject);
        }

        Destroy(gameObject);
    }
}
