using System.Collections;
using UnityEngine;

namespace WeaponSys {
    public class Gun : MonoBehaviour, IWeapon {
        public float cooldownDuration = 0.8f;
        public float shootDistance = 1.0f;

        private bool isCoolingDown;

        public bool Attack(float shootingAngle) {
            if (!CanShoot()) {
                return false;
            }

            if (!cooldownDuration.Equals(0.0f)) {
                isCoolingDown = true;
                StartCoroutine(CooldownRoutine(cooldownDuration));
            }

            LayerMask mask = LayerMask.GetMask("Entities") |
                             LayerMask.GetMask("Ground")|
                             LayerMask.GetMask("Player");

            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                new Vector2(- (shootingAngle - 90) / 90, 0),
                shootDistance,
                mask);

            if (hit.collider != null && hit.collider.TryGetComponent(out IDamageable damageable)) {
                damageable.Damage(DamageType.BulletHit);
            }

            return true;
        }

        private bool CanShoot() => !isCoolingDown;

        private IEnumerator CooldownRoutine(float duration) {
            yield return new WaitForSeconds(duration);
            isCoolingDown = false;
        }
    }
}
