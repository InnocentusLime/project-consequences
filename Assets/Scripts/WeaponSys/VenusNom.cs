﻿using System;
using Characters;
using Extensions;
using UnityEngine;

namespace WeaponSys {
    public class VenusNom: MonoBehaviour, IWeapon {
        private IDamageable owner;

        private void Awake() {
            owner = GetComponent<IDamageable>();
        }

        public bool Attack(GameObject obj) {
            if (!obj.TryGetComponent(out IEdible edible)) {
                return false;
            }

            edible.Damage(DamageType.VenusEat);

            switch (edible.GetFoodType()) {
                case FoodType.Harmless:
                    break;
                case FoodType.Poisonous:
                    owner.Damage(DamageType.FoodPoison);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}
