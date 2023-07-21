using UnityEngine;

public enum FoodType {
    Harmless,
    Poisonous
}

public interface IEdible: IDamageable {
    public FoodType GetFoodType();
}
