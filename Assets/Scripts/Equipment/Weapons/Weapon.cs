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
    public bool ignoreOwner = true;
    public float weaponRange = 0;

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

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Clamp values to avoid negative stats
        physicalDamage = Mathf.Max(0, physicalDamage);
        elementalDamage = Mathf.Max(0, elementalDamage);
        attackRate = Mathf.Max(0.01f, attackRate);
        manaCost = Mathf.Max(0, manaCost);
        weaponRange = Mathf.Max(0, weaponRange);

        if (weaponType == WeaponType.Melee || weaponType == WeaponType.Ranged)
        {
            manaCost = 0;
        }

        if (weaponType == WeaponType.Melee)
        {
            weaponRange = 0;
        }

        if (weaponType != WeaponType.Ranged && projectilePrefab != null)
        {
            projectilePrefab = null;
        }

        if (weaponType != WeaponType.Melee && weaponType != WeaponType.Magic && damageZonePrefab != null)
        {
            damageZonePrefab = null;
        }

        // Warn if projectilePrefab is missing for ranged weapons
        if (weaponType == WeaponType.Ranged && projectilePrefab == null)
        {
            Debug.LogWarning($"[Weapon SO: {name}] Ranged weapon without projectilePrefab assigned!", this);
        }

        // Warn if damageZonePrefab is missing for melee or magic weapons
        if ((weaponType == WeaponType.Melee || weaponType == WeaponType.Magic) && damageZonePrefab == null)
        {
            Debug.LogWarning($"[Weapon SO: {name}] {weaponType} weapon without damageZonePrefab assigned!", this);
        }
    }
#endif

}
