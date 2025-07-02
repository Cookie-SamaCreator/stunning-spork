using UnityEngine;

namespace RPG.Player
{
    public class FirePointFollower : MonoBehaviour
    {
        [Tooltip("The transform representing the fire point (e.g., gun barrel).")]
        public Transform firePoint;

        [Tooltip("The camera used to convert mouse position to world coordinates.")]
        public Camera cam;

        [Tooltip("Distance from the player to place the fire point.")]
        public float firePointDistance = 1.5f;

        private void Update()
        {
            // Get mouse position in world space
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f; // Ensure z is zero for 2D

            // Calculate direction from player to mouse
            Vector3 direction = (mouseWorldPos - transform.position).normalized;

            // Set fire point position at a fixed distance in the direction of the mouse
            firePoint.position = transform.position + direction * firePointDistance;

            // Rotate fire point to face the mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
