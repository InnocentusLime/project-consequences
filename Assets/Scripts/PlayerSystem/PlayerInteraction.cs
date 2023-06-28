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

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerable<Interactable> GetTouchedInteractable() {
        int count = interactionZone.OverlapCollider(interactableLayerFilter, collisionBuffer);

        return collisionBuffer.Take(count)
            .Select(x => x.gameObject.GetComponent<Interactable>());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            foreach (Interactable interactable in GetTouchedInteractable()) {
                interactable.interactionEvent.Invoke(gameObject);
            }
        }
    }
}
