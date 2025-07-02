using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockbackDuration = 0.2f; // Duration of the knockback effect
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(GameObject damageSource, float knockbackForce)
    {
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.transform.position).normalized * knockbackForce * rb.mass;

        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockbackDuration); // Adjust the duration of the knockback effect
        rb.linearVelocity = Vector2.zero; // Stop the knockback effect
        GettingKnockedBack = false;
    }
}
