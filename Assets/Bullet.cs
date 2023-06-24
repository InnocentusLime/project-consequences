using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;

    void Start()
    {
        Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();

        rigidBody2D.velocity = new Vector2((float)(moveSpeed * Math.Cos(Mathf.PI / 2 + rigidBody2D.rotation)),
            (float)(moveSpeed * Math.Sin(Mathf.PI / 2 + rigidBody2D.rotation)));
    }
}
