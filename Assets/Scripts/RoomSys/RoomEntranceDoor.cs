using UnityEngine;

namespace RoomSys {
    public class RoomEntranceDoor : MonoBehaviour {
        private void Awake() {
            GlobalRoomState.playerEnterEvent.AddListener(OnPlayerEnter);
        }

        private void OnDestroy() {
            GlobalRoomState.playerEnterEvent.RemoveListener(OnPlayerEnter);
        }

        private void OnPlayerEnter() {
            GlobalRoomState.player.transform.position = transform.position;
            GlobalRoomState.player.SetActive(true);
        }
    }
}
