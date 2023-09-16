using UnityEngine;

namespace RoomSys {
    public class ShadowDirector : MonoBehaviour {
        public ShadowBehaviour shadowPrefab;

        public void OnPlayerShoot(Vector2 shootDirection, Vector2 position) {
            ShadowBehaviour shadow = Instantiate(
                shadowPrefab,
                position,
                Quaternion.identity);

            shadow.LookInDirection(shootDirection);
        }
    }
}
