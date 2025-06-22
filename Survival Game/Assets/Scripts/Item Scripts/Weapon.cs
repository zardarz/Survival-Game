
using UnityEngine;

public class Weapon : Item
{
    [Header("Weapon")]
    [Range(0,30)]
    [SerializeField] private float weaponSpeed;
    [SerializeField] private float weaponDamage;

    public float GetWeaponSpeed() {
        return weaponSpeed;
    }

    public float GetWeaponDamage() {
        return weaponDamage;
    }
}