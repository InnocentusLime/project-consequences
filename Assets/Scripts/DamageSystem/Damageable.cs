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

[System.Serializable]
public class DamageEvent : UnityEvent<DamageType> { }

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private DamageEvent damageEvent;

    public void Damage(DamageType damageType)
    {
        damageEvent.Invoke(damageType);
    }
}
