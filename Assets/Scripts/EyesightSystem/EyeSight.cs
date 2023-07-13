using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

[Serializable]
public class ObjectFoundEvent : UnityEvent<GameObject> {
}

public class EyeSight : MonoBehaviour {
    public Vector2 sightDirection;
    public Vector2 rayOffset;

    [SerializeField] private float rayLength;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightAngleStep;
    [SerializeField] private LayerMask sightMask;
    [SerializeField] private LayerMask reportMask;
    [SerializeField] private ObjectFoundEvent objectFoundEvent;

    void FixedUpdate() {
        float start = -sightAngle / 2;
        float end = sightAngle / 2;

        Vector2 startRaycast = new Vector2(transform.position.x + rayOffset.x,
            transform.position.y + rayOffset.y);

        for (; start <= end; start += sightAngleStep) {
            Vector2 newDirection = new Vector2(sightDirection.x * Mathf.Cos(start * Mathf.Deg2Rad),
                sightDirection.x * Mathf.Sin(start * Mathf.Deg2Rad));

            Debug.DrawLine(startRaycast, startRaycast + newDirection * rayLength, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(startRaycast, newDirection, rayLength, sightMask);

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & reportMask) != 0) {
                objectFoundEvent.Invoke(hit.collider.gameObject);
                break;
            }
        }
    }
}
