using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Just destroy the bullet, let the enemy handle its own death logic
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("LineOfSight")) // skip LineOfSight trigger
        {
            Destroy(gameObject);
        }
    }
}
