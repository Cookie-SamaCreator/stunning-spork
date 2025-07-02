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
        public GameObject projectilePrefab;

        public override void Equip(PlayerStats player)
        {
            foreach (var mod in statModifiers)
            {
                if (!player.ModifyStat(mod.statName, mod.value))
                {
                    Debug.LogWarning("stat name doesn't exist");
                }
            }
        }

        public override void Unequip(PlayerStats player)
        {
            foreach (var mod in statModifiers)
            {
                if (!player.ModifyStat(mod.statName, -mod.value))
                {
                    Debug.LogWarning("stat name doesn't exist");
                }
            }
        }
    }
