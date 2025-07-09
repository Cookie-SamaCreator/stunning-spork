using RPG.Combat;
using RPG.Definitions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX; // TODO Add VFX
    private bool isEnemyProjectile; // TODO Implement some mechanic about enemy projectiles (to avoid Friendly Fire)
    private float projectileRange;
    private DamageData damage;
    private Vector3 startPosition;
    public void Init(DamageData dmg, float range, bool isEnemyProjectile = false)
    {
        damage = dmg;
        projectileRange = range;
        this.isEnemyProjectile = isEnemyProjectile;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        // if the target has a DamageController -> it can be damaged
        if (collision.TryGetComponent(out DamageController dc))
        {
            dc.ApplyDamage(damage);
        }

        if (particleOnHitPrefabVFX != null)
        {
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
    }
}
