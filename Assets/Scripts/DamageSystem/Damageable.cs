using UnityEngine;
using UnityEngine.Events;

public enum DamageType
{
    BulletHit,
    DecayDamage,
    FoodPoison,
    VenusEat,
    ZombiePunch
}

public interface IDamageable {
    public void Damage(DamageType damageType);
}
