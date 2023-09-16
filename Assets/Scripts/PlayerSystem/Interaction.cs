using UnityEngine;

public class Interaction : MonoBehaviour {
    private readonly Collider2D[] collisionBuffer = new Collider2D[3];
    private ContactFilter2D interactableLayerFilter;

    private void Awake() {
        interactableLayerFilter.layerMask = LayerMask.GetMask("Interaction");
        interactableLayerFilter.useLayerMask = true;
        interactableLayerFilter.useTriggers = true;
    }

    // TODO TryGetComponent is better than `GetComponent` it seems. See where we can place it
    // TODO fix in the future to make interaction collider more editable
    public void Interact() {
        int interactionCount = Physics2D.OverlapBox(
            transform.position,
            new Vector2(1.5f, .75f),
            0f,
            interactableLayerFilter,
            collisionBuffer);

        for (int i = 0; i < interactionCount; ++i) {
            bool gotInteractable = collisionBuffer[i].TryGetComponent(out Interactable interactable);
            if (gotInteractable) {
                interactable.interactionEvent.Invoke(gameObject);
            }
        }
    }
}
