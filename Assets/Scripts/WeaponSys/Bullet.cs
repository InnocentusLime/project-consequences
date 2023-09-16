using UnityEngine;

namespace WeaponSys {
    public class Bullet : MonoBehaviour {
        public float moveSpeed;
        public float lifeTime;
        public GameObject creator;

        private void Start() {
            Rigidbody2D rigidBody2D = GetComponent<Rigidbody2D>();

            float angle = (rigidBody2D.rotation + 90) * Mathf.Deg2Rad;
            rigidBody2D.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * moveSpeed;

            Destroy(gameObject, lifeTime);
        }

        // TODO make it a "onCollisionEnter2D" one day?
        private void OnTriggerEnter2D(Collider2D col) {
            GameObject colGameObject = col.gameObject;

            // Another ugly bodge!!!
            LayerMask objectLayer = 1 << colGameObject.layer;
            LayerMask mask = LayerMask.GetMask("Entities") |
                             LayerMask.GetMask("Ground")|
                             LayerMask.GetMask("Player");
            if (colGameObject == creator || (mask & objectLayer) == 0) {
                return;
            }

            if (colGameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.Damage(DamageType.BulletHit);
            }

            Destroy(gameObject);
        }
    }
}
