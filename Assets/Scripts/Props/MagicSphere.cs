using UnityEngine;
using WeaponSys;

namespace Props {
    public class MagicSphere : MonoBehaviour, IDamageable {
        public GameObject[] targets;

        private bool isActive;
        private SpriteRenderer spriteRenderer;

        public void Awake() {
            foreach (GameObject target in targets) {
                target.SetActive(false);
            }

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Activate() {
            if (isActive) {
                return;
            }

            foreach (GameObject target in targets) {
                target.SetActive(true);
            }

            spriteRenderer.color = new Color(0.0f, 0.0f, 0.3f);
            isActive = true;
        }

        public void Damage(DamageType damageType) {
            if (damageType == DamageType.BulletHit) {
                Activate();
            }
        }
    }
}
