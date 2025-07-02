using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageZone>() || other.gameObject.GetComponent<Projectile>())
        {
            PickupSpawner pickupSpawner = GetComponent<PickupSpawner>();
            pickupSpawner?.DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
