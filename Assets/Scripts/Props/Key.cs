using Extensions;
using RoomSys;
using UnityEngine;

namespace Props {
    public class Key : MonoBehaviour, IInteractable {
        private RoomExitDoor doorToUnlock;

        private void Awake() {
            doorToUnlock = FindFirstObjectByType<RoomExitDoor>();
        }

        public void OnInteraction(GameObject actor) {
            Destroy(gameObject);
            GlobalRoomState.startConsequenceTimeEvent.Invoke();
            doorToUnlock.Unlock();
        }
    }
}
