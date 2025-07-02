using RPG.Combat;
using RPG.Definitions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    private DamageData damage;
    private Vector3 startPosition;
    public void Init(DamageData dmg) => damage = dmg;

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

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
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
