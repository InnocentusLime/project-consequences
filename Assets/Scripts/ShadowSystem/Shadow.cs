using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : CursedBehaviour {
    public float shootAngle;
    public float visionLength;

    private Gun gun;
    private ContactFilter2D filter;
    private readonly RaycastHit2D[] seenObjects = new RaycastHit2D[3];

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Entities");
    }

    private void Update() {
        int count = Physics2D.BoxCast(
            transform.localPosition,
            new Vector2(0.8f, 0.3f),
            shootAngle,
            new Vector2(
                Mathf.Cos((shootAngle + 90) * Mathf.Deg2Rad),
                Mathf.Sin((shootAngle + 90) * Mathf.Deg2Rad)
                ),
            filter,
            seenObjects,
            visionLength
        );

        for (int i = 0; i < count; ++i) {
            GameObject seenObject = seenObjects[i].rigidbody.gameObject;

            if (seenObject == GlobalRoomState.player) {
                gun.Shoot(shootAngle);
                Destroy(gameObject);
            }
        }
    }

    protected override void OnConsequenceTime() {
        gameObject.SetActive(true);
    }

    protected override void OnMadnessChange(int madnessLevel) {
        throw new System.NotImplementedException();
    }
}
