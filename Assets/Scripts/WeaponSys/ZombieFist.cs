using UnityEngine;

namespace WeaponSys {
    public class ZombieFist: MonoBehaviour, IWeapon {
        private static readonly LayerMask mask = 1 << 10; // Must be "Player"
        private readonly Collider2D[] collisionBuff = new Collider2D[3];

        public bool Attack(float angle) {
            int numRes = Physics2D.OverlapBoxNonAlloc(
                transform.position,
                new Vector2(1.1f, 1f),
                0,
                collisionBuff,
                mask);
            for (int i = 0; i < numRes; i++) {
                Attack(collisionBuff[i].gameObject);
            }

            return numRes > 0;
        }

        public bool Attack(GameObject obj) {
            if (!obj.TryGetComponent(out IDamageable dmg)) {
                return false;
            }

            dmg.Damage(DamageType.ZombiePunch);
            return true;
        }
    }
}
