using WeaponSys;

namespace Extensions {
    public enum FoodType {
        Harmless,
        Poisonous
    }

    public interface IEdible: IDamageable {
        public FoodType GetFoodType();
    }
}
