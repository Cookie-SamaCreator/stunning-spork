using UnityEngine;

namespace RPG.Definitions
{
    public struct DamageData
    {
        public int physicalDamage;
        public ElementType element;
        public int elementalDamage;
        public GameObject source;
        public GameObject damageZone;
        public bool ignoreOwner;

        public DamageData(int phys, ElementType elem, int elemDmg, GameObject dmgZone = null, GameObject src = null, bool ownerIgnore = true)
        {
            physicalDamage = phys;
            element = elem;
            elementalDamage = elemDmg;
            damageZone = dmgZone;
            source = src;
            ignoreOwner = ownerIgnore;
        } 
    }
}
