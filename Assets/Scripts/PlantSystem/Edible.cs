using UnityEngine;

public enum FoodType
{
    Harmless,
    Poisonous
}

public class Edible : MonoBehaviour {
    [SerializeField]
    private FoodType foodType;

    public FoodType GetFoodType()
    {
        return foodType;
    }

    public void GetEaten(DamageType damageType)
    {
        if (TryGetComponent<Damageable>(out Damageable damageable))
        {
            damageable.Damage(damageType);
        }
    }
}