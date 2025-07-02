using RPG.Combat;
using RPG.Definitions;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private DamageData damage;

    public void Init(DamageData dmg) => damage = dmg;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out DamageController dc))
        {
            dc.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }
}
