using UnityEngine;
using RPG.Definitions;

/// <summary>
/// An AOE is a damageZone with a lifespan
/// </summary>
public class DamageAOE : DamageZone
{
    [SerializeField] private float lifeTime = 0.2f;

    public override void Init(DamageData dmg, GameObject owner)
    {
        base.Init(dmg, owner);
        Destroy(gameObject, lifeTime);
    }
}