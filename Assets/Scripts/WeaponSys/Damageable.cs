namespace WeaponSys {
    public enum DamageType {
        BulletHit,
        FoodPoison,
        VenusEat,
        ZombiePunch
    }

    public interface IDamageable {
        public void Damage(DamageType damageType);
    }
}
