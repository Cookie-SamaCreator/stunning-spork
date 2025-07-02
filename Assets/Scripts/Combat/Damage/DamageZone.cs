using UnityEngine;
using RPG.Combat;
using RPG.Definitions;

/// <summary>
/// Used to describe weapon damage zone (sword collider for example)
/// </summary>
public class DamageZone : MonoBehaviour
{
    private DamageData damageData;
    private GameObject owner;
    bool ignoreOwner = false;
    public virtual void Init(DamageData dmg, GameObject owner, bool ignoreOwner = false)
    {
        this.damageData = dmg;
        this.owner = owner;
        this.ignoreOwner = ignoreOwner;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == owner && ignoreOwner) return;

        if (other.TryGetComponent(out DamageController dc))
        {
            dc.ApplyDamage(damageData);
        }
    }
}
