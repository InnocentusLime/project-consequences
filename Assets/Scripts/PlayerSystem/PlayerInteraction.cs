using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteraction : MonoBehaviour {
    [FormerlySerializedAs("trigger")] [SerializeField] private Collider2D interactionZone;

    private readonly Collider2D[] collisionBuffer = new Collider2D[3];
    private ContactFilter2D interactableLayerFilter;

    private void Awake() {
        interactableLayerFilter.layerMask = LayerMask.GetMask("Interaction");
        interactableLayerFilter.useLayerMask = true;
        interactableLayerFilter.useTriggers = true;
    }

    private void InteractAround() {
        int count = interactionZone.OverlapCollider(interactableLayerFilter, collisionBuffer);

        IEnumerable<Interactable> touchedIntractable = collisionBuffer.Take(count)
            .Select(x => x.gameObject.GetComponent<Interactable>());

        foreach (Interactable interactable in touchedIntractable) {
            interactable.interactionEvent.Invoke(gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            InteractAround();
        }
    }
}
