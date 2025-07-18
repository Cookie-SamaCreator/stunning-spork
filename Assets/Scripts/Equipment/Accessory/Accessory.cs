using UnityEngine;

    [CreateAssetMenu(menuName = "Equipment/Accessory")]
    public class Accessory : Equipment
    {
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
