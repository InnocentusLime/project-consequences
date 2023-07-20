//#define DEBUG_EYESIGHT_RAYS

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
    public LayerMask sightMask;
    public LayerMask reportMask;

    [SerializeField] private float rayLength;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightAngleStep;
    [SerializeField] private ObjectFoundEvent objectFoundEvent;

    private void FixedUpdate() {
        Vector2 raycastStart = (Vector2)transform.position + rayOffset;

        for (float rayAngle = -sightAngle / 2f; rayAngle <= sightAngle / 2f; rayAngle += sightAngleStep) {
            Vector2 raycastDirection = new Vector2(sightDirection.x * Mathf.Cos(rayAngle * Mathf.Deg2Rad),
                sightDirection.x * Mathf.Sin(rayAngle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(raycastStart, raycastDirection, rayLength, sightMask);

#if DEBUG_EYESIGHT_RAYS
            Debug.DrawLine(
                raycastStart,
                raycastStart + raycastDirection * (hit ? hit.distance : rayLength),
                Color.red
            );
#endif

            if (!hit) {
                continue;
            }

            int objectLayerMask = 1 << hit.collider.gameObject.layer;
            if ((objectLayerMask & reportMask) == 0) {
                continue;
            }

            objectFoundEvent.Invoke(hit.collider.gameObject);
            break;
        }
    }
}
