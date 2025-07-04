using UnityEngine;
using System.Collections.Generic;
using RPG.Definitions;

namespace RPG.Systems
{
    public class EquipmentManager : Singleton<EquipmentManager>
    {
        [Header("Current Equipment")]
        private Dictionary<EquipmentSlot, Equipment> equippedItems = new();
        private ActiveWeapon activeWeapon;
        private PlayerStats stats;

        protected override void Awake()
        {
            base.Awake();
            stats = GetComponent<PlayerStats>();
            activeWeapon = GetComponentInChildren<ActiveWeapon>();
            equippedItems.Clear();
            foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
            {
                equippedItems[slot] = null;
            }
        }

        public void UpdateWeapon(Weapon newWeapon)
        {
            Equip(newWeapon, EquipmentSlot.Weapon);
        }

        /// <summary>
        /// Returns the equipment currently equipped in the given slot.
        /// </summary>
        public Equipment GetEquipped(EquipmentSlot slot)
        {
            equippedItems.TryGetValue(slot, out Equipment item);
            return item;
        }

        /// <summary>
        /// Returns the currently equipped weapon, or null if none.
        /// </summary>
        public Weapon GetEquippedWeapon()
        {
            return GetEquipped(EquipmentSlot.Weapon) as Weapon;
        }

        /// <summary>
        /// Equips the given equipment in the specified slot.
        /// </summary>
        public void Equip(Equipment equipment, EquipmentSlot slot)
        {
            if (equipment == null) return;

            if (equippedItems.TryGetValue(slot, out Equipment previous) && previous != null)
            {
                previous.Unequip(stats);
            }

            equippedItems[slot] = equipment;
            equipment.Equip(stats);

            /*if (slot == EquipmentSlot.Weapon && activeWeapon != null)
            {
                activeWeapon.NewWeapon(equipment as Weapon);
            }*/

            Debug.Log($"Equipped {equipment.EquipmentName} in {slot}.");
        }

        /// <summary>
        /// Unequips the equipment from the specified slot.
        /// </summary>
        public void Unequip(EquipmentSlot slot)
        {
            if (equippedItems.TryGetValue(slot, out Equipment equipment) && equipment != null)
            {
                equipment.Unequip(stats);
                equippedItems[slot] = null;

                /*if (slot == EquipmentSlot.Weapon && activeWeapon != null)
                {
                    activeWeapon.WeaponNull();
                }*/

                Debug.Log($"Unequipped {slot}.");
            }
        }

        /// <summary>
        /// Unequips all equipment.
        /// </summary>
        public void UnequipAll()
        {
            foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
            {
                Unequip(slot);
            }
        }
    }
}
