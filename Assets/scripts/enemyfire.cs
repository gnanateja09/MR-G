using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float shootInterval = 2f;
    public float visionRange = 30f;
    public LayerMask obstacleMask;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;     // Muzzle flash particle
    public AudioSource gunShotAudio;       // Gunshot sound

    private float timer;
    private GameObject player;
    private Transform playerHead;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHead = player.transform.Find("Head") ?? player.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null || playerHead == null)
            return;

        timer += Time.deltaTime;

        if (timer >= shootInterval && CanSeePlayer())
        {
            ShootForward();
            timer = 0f;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (playerHead.position - firePoint.position).normalized;
        float distanceToPlayer = Vector3.Distance(firePoint.position, playerHead.position);

        if (distanceToPlayer > visionRange)
            return false;

        if (Physics.Raycast(firePoint.position, directionToPlayer, out RaycastHit hit, visionRange, ~obstacleMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    void ShootForward()
    {
        if (isDead) return; // Ensure dead enemies don't shoot

        if (!enemyBulletPrefab || !firePoint) return;

        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
            Debug.Log("Playing muzzle");
        }

        // Play gunshot sound
        if (gunShotAudio != null)
        {
            gunShotAudio.Play();
            Debug.Log("Playing Gunshot")
;        }

        // Instantiate and shoot bullet
        GameObject bullet = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.forward * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }

    public void Die()
    {
        isDead = true;

        // Stop effects if playing
        if (muzzleFlash != null && muzzleFlash.isPlaying)
        {
            muzzleFlash.Stop();
        }

        if (gunShotAudio != null && gunShotAudio.isPlaying)
        {
            gunShotAudio.Stop();
        }

        // Optional: disable this component to ensure no further logic runs
        this.enabled = false;
    }
}