using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : CursedBehaviour {
    public float shootAngle;
    public float visionLength;
    public float scanRange = 5.0f;

    private Gun gun;
    private ContactFilter2D filter;
    private readonly RaycastHit2D[] seenObjects = new RaycastHit2D[3];

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        filter.useLayerMask = true;
        filter.layerMask = LayerMask.GetMask("Entities");
    }

    private void Update() {
        // FIXME hacky way to fetch bullet size. This might be greatly inaccurate.
        Vector2 bulletSize = gun.bulletPrefab.transform.localScale;

        for (float angDelta = -scanRange; angDelta <= scanRange; angDelta += 1f) {
            float angle = shootAngle + angDelta;

            int count = Physics2D.BoxCast(
                transform.localPosition,
                bulletSize * 0.45f,
                angle,
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
                    gun.Shoot(angle);
                    Destroy(gameObject);
                }
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
