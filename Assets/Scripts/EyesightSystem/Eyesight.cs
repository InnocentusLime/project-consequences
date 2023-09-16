// #define DEBUG_EYESIGHT_RAYS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public interface IEyesightClient {
    public void OnSeenObject(GameObject obj);

    public LayerMask GetSightMask();
    public LayerMask GetReportMask();
}

public class Eyesight : MonoBehaviour {
    public Vector2 rayOffset;

    private Vector2 implSightDirection = Vector2.right;
    public Vector2 sightDirection {
        get => implSightDirection;
        set => implSightDirection = value.normalized;
    }

    [SerializeField] private float rayLength;
    [SerializeField] private float sightAngle;
    [SerializeField] private float sightAngleStep;

    private IEyesightClient client;

    private void Awake() {
        client = GetComponent<IEyesightClient>();
    }

    private void FixedUpdate() {
        LayerMask sightMask = client.GetSightMask();
        LayerMask reportMask = client.GetReportMask();
        Vector2 raycastStart = (Vector2)transform.position + rayOffset;

        if (sightMask == 0 || reportMask == 0) {
            return;
        }

        Assert.IsTrue(sightAngleStep > 0.01f);
        Assert.IsTrue(sightAngle > 0f);

        for (float rayAngle = -sightAngle / 2f; rayAngle <= sightAngle / 2f; rayAngle += sightAngleStep) {
            Vector2 raycastDirection = Quaternion.AngleAxis(rayAngle, Vector3.forward) * implSightDirection;

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

            client.OnSeenObject(hit.collider.gameObject);
        }
    }
}
