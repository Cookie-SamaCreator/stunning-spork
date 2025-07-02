using UnityEngine;
using System.Collections.Generic;
using RPG.Definitions;

namespace RPG.Systems
{
    public class EquipmentManager : MonoBehaviour
    {
        [Header("Current Equipment")]
        private Dictionary<EquipmentSlot, Equipment> equippedItems = new();

        private PlayerStats stats;

        private void Awake()
        {
            stats = GetComponent<PlayerStats>();

            foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
            {
                equippedItems[slot] = null;
            }
        }

        public Equipment GetEquipped(EquipmentSlot slot)
        {
            equippedItems.TryGetValue(slot, out Equipment item);
            return item;
        }

        public void Equip(Equipment equipment, EquipmentSlot slot)
        {
            if (equipment == null) { return; }

            if (equippedItems.TryGetValue(slot, out Equipment previous) && previous != null)
            {
                previous.Unequip(stats);
            }

            equippedItems[slot] = equipment;
            equipment.Equip(stats);

            Debug.Log($"Equipped {equipment.EquipmentName} int {slot}.");
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (equippedItems.TryGetValue(slot, out Equipment equipment) && equipment != null)
            {
                equipment.Unequip(stats);
                equippedItems[slot] = null;

                Debug.Log($"Unequipped {slot}.");
            }
        }

        public void UnequipAll()
        {
            foreach (EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentSlot)))
            {
                Unequip(slot);
            }
        }
    }
}
