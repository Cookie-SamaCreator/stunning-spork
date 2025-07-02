using RPG.Definitions;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment/Weapon")]
public class Weapon : Equipment
{
    public WeaponType weaponType;
    public int physicalDamage = 10;
    public ElementType elementType = ElementType.None;
    public int elementalDamage = 0;
    public float attackRate = 1f;
    public int manaCost = 0;

    [Header("Projectile (Ranged only)")]
    public GameObject projectilePrefab;

    [Header("Damage Zone (Melee and Magic)")]
    public GameObject damageZonePrefab;

    public override void Equip(PlayerStats player)
    {
        foreach (var mod in statModifiers)
        {
            if (!player.ModifyStat(mod.statName, mod.value))
            {
                Debug.LogWarning("Stat name doesn't exist");
            }
        }
    }

    public override void Unequip(PlayerStats player)
    {
        foreach (var mod in statModifiers)
        {
            if (!player.ModifyStat(mod.statName, -mod.value))
            {
                Debug.LogWarning("Stat name doesn't exist");
            }
        }
    }
}
