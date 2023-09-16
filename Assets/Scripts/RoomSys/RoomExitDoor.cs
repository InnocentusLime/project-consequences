using Extensions;
using UnityEngine;

namespace RoomSys {
    public class RoomExitDoor : MonoBehaviour, IInteractable {
        [SerializeField] private bool isLocked = true;
        private SpriteRenderer spriteRenderer;

        private void Awake() {
            // NOTE remove ASAP. This for a more testable prototype
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = isLocked ? new Color(0.0f, 0.3f, 0.0f) : new Color(0.0f, 1.0f, 0.0f);
        }

        public void OnInteraction(GameObject actor) {
            if (isLocked) {
                return;
            }

            actor.SetActive(false);
            GlobalRoomState.playerLeaveEvent.Invoke();
        }

        public void Unlock() {
            isLocked = false;
            spriteRenderer.color = new Color(0.0f, 1.0f, 0.0f);
        }
    }
}
