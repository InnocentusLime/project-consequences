using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : CursedBehaviour {
    public float shootAngle;
    public float visionLength;
    public float scanRange = 5.0f;

    private Gun gun;
    int layerMask;

    protected override void ExtraAwake() {
        gun = GetComponent<Gun>();
        layerMask = LayerMask.GetMask("Entities") | LayerMask.GetMask("Ground");
    }

    private void Update() {
        // FIXME hacky way to fetch bullet size. This might be greatly inaccurate.
        Vector2 bulletSize = gun.bulletPrefab.transform.localScale;

        for (float angDelta = -scanRange; angDelta <= scanRange; angDelta += 1f) {
            float angle = shootAngle + angDelta;

            GameObject seenObject = Physics2D.BoxCast(
                transform.localPosition,
                bulletSize * 0.45f,
                angle,
                new Vector2(
                    Mathf.Cos((shootAngle + 90) * Mathf.Deg2Rad),
                    Mathf.Sin((shootAngle + 90) * Mathf.Deg2Rad)
                ),
                visionLength,
                layerMask
            ).rigidbody.gameObject;

            if (seenObject == GlobalRoomState.player) {
                gun.Shoot(angle);
                Destroy(gameObject);

                // I have accidentally made the shadow shoot several bullets at once
                // On second thought, maybe we should consider giving it a shotgun...
                // And then give the shotgun to the player in the very end so they can
                // DESTROY everyone.
                break;
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
