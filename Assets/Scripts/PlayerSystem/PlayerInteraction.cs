using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Collider2D interactionZone;

    private readonly Collider2D[] collisionBuffer = new Collider2D[3];
    private ContactFilter2D interactableLayerFilter;

    private void Awake() {
        interactableLayerFilter.layerMask = LayerMask.GetMask("Interaction");
        interactableLayerFilter.useLayerMask = true;
        interactableLayerFilter.useTriggers = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            int interactionCount = interactionZone.OverlapCollider(interactableLayerFilter, collisionBuffer);
            for (int i = 0; i < interactionCount; ++i) {
                Interactable interactable = collisionBuffer[i].GetComponent<Interactable>();
                if (interactable != null) {
                    interactable.interactionEvent.Invoke(gameObject);
                }
            }
        }
    }
}
