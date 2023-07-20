using UnityEngine;

public enum FoodType
{
    Harmless,
    Poisonous
}

public interface IEdible {
    public FoodType GetFoodType();

    public void GetEaten(DamageType damageType);
}
