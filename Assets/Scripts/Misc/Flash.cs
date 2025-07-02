using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float flashDuration = 0.2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public float GetFlashDuration()
    {
        return flashDuration;
    }

    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMat; // Set the flash material
        yield return new WaitForSeconds(flashDuration); // Wait for the specified duration
        spriteRenderer.material = defaultMat; // Restore the default material
    }
}
