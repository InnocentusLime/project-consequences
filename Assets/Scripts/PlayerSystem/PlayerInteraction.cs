using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    [SerializeField] private Collider2D trigger;
    private Collider2D[] results = new Collider2D[3];
    private ContactFilter2D interactableFilter;

    private void Awake() {
        interactableFilter.layerMask = LayerMask.GetMask("Interaction");
        interactableFilter.useLayerMask = true;
        interactableFilter.useTriggers = true;
    }

    private void TryInteract() {
        int count = trigger.OverlapCollider(interactableFilter, results);

        for (int i = 0; i < count; ++i) {
            results[i].gameObject.SendMessage("OnInteract", gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            TryInteract();
        }
    }
}
