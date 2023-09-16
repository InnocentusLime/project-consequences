using System.Collections;
using UnityEngine;

namespace WeaponSys {
    public class Gun : MonoBehaviour, IWeapon {
        public float cooldownDuration = 0.8f;
        public Bullet bulletPrefab;

        private bool isCoolingDown;

        public bool Attack(float shootingAngle) {
            if (!CanShoot()) {
                return false;
            }

            if (!cooldownDuration.Equals(0.0f)) {
                isCoolingDown = true;
                StartCoroutine(CooldownRoutine(cooldownDuration));
            }

            Bullet bullet = Instantiate(
                bulletPrefab,
                transform.position,
                Quaternion.AngleAxis(shootingAngle - 90, Vector3.forward)
            );
            bullet.creator = gameObject;

            return true;
        }

        private bool CanShoot() => !isCoolingDown;

        private IEnumerator CooldownRoutine(float duration) {
            yield return new WaitForSeconds(duration);
            isCoolingDown = false;
        }
    }
}
