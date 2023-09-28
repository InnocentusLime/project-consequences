// #define DEBUG_BULLET_RAYS

using System.Collections;
using Characters;
using UnityEngine;

namespace WeaponSys {
    public class Gun : MonoBehaviour, IWeapon {
        public float cooldownDuration = 0.8f;
        public float shootDistance = 10.0f;

        private bool isCoolingDown;

        private Collider2D shooter;

        private readonly RaycastHit2D[] hits = new RaycastHit2D[1];
        private readonly LayerMask collisionMask = (1 << 6) | (1 << 9) | (1 << 10); // Must be "Ground | Entity | Player"

        private void Awake() {
            shooter = gameObject.GetComponent<Collider2D>();
        }

        public bool Attack(float shootingAngle) {
            if (!CanShoot()) {
                return false;
            }

            isCoolingDown = true;
            StartCoroutine(CooldownRoutine(cooldownDuration));

            shootingAngle *= Mathf.Deg2Rad;
            Vector2 shootingDirection = new Vector2(Mathf.Cos(shootingAngle), Mathf.Sin(shootingAngle));
            int objects = shooter.Raycast(shootingDirection, hits, shootDistance, collisionMask);

#if DEBUG_BULLET_RAYS
            Debug.DrawLine(
                transform.position,
                (Vector2)transform.position + shootDistance * shootingDirection,
                Color.red, 1
            );
#endif

            if (objects != 0 && hits[0].collider.TryGetComponent(out IDamageable damageable)) {
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
