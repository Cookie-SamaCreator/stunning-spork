using UnityEngine;

namespace RPG.Definitions
{
    public struct DamageData
    {
        public int physicalDamage;
        public ElementType element;
        public int elementalDamage;
        public GameObject source;

        public DamageData(int phys, ElementType elem, int elemDmg, GameObject src = null)
        {
            physicalDamage = phys;
            element = elem;
            elementalDamage = elemDmg;
            source = src;
        } 
    }
}
