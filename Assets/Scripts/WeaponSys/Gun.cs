#define DEBUG_BULLET_WAY

using System.Collections;
using UnityEngine;

namespace WeaponSys {
    public class Gun : MonoBehaviour, IWeapon {
        public float cooldownDuration = 0.8f;
        public float shootDistance = 10.0f;

        private bool isCoolingDown;
        private Collider2D shooter;

        private readonly RaycastHit2D[] hits = new RaycastHit2D[1];

        private void Awake() {
            shooter = gameObject.GetComponent<Collider2D>();
        }

        public bool Attack(float shootingAngle) {
            if (!CanShoot()) {
                return false;
            }

            if (!cooldownDuration.Equals(0.0f)) {
                isCoolingDown = true;
                StartCoroutine(CooldownRoutine(cooldownDuration));
            }

            LayerMask mask = LayerMask.GetMask("Entities") |
                             LayerMask.GetMask("Ground") |
                             LayerMask.GetMask("Player");

            float deg2Rad = shootingAngle * Mathf.Deg2Rad;
            int objects = shooter.Raycast(new Vector2(Mathf.Cos(deg2Rad), Mathf.Sin(deg2Rad)), hits, shootDistance,
                mask);

#if DEBUG_BULLET_WAY
Debug.DrawLine(shooter.transform.position,
                new Vector3(shooter.transform.position.x + shootDistance * Mathf.Cos(deg2Rad),
                            shooter.transform.position.y+ shootDistance * Mathf.Sin(deg2Rad), 0),
                new Color(1, 0, 0), 1);
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
