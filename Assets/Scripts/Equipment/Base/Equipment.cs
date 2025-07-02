using UnityEngine;
using System.Collections.Generic;
using RPG.Definitions;

public abstract class Equipment : ScriptableObject
{
    public string EquipmentName;
    public List<StatModifier> statModifiers;

    public abstract void Equip(PlayerStats player);
    public abstract void Unequip(PlayerStats player);
}
