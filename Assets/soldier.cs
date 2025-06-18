using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Soldier : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject player;
    private EnemyShooter fireScript; // Updated script name

    [Header("Movement Settings")]
    public float moveSpeed = 3.5f;
    public float stoppingDistance = 2f;

    [Header("Shooting Settings")]
    public float shootInterval = 2f;
    private float shootTimer = 0f;

    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;

        player = GameObject.FindGameObjectWithTag("Player");

        // Updated to match script name
        fireScript = GetComponentInChildren<EnemyShooter>();

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure it has the tag 'Player'");
        }
    }

    void Update()
    {
        if (isDead || player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        shootTimer += Time.deltaTime;

        if (distance > stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;

            if (shootTimer >= shootInterval)
            {
                animator.SetTrigger("Shoot");
                shootTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("PlayerBullet"))
        {
            isDead = true;
            agent.isStopped = true;

            animator.SetTrigger("Die");

            // Stop bullet spawning
            EnemyShooter shooter = GetComponentInChildren<EnemyShooter>();
            if (shooter != null && Input.GetKeyDown("space"))
            {
                Debug.Log("Calling Death");
                shooter.Die(); // Stop shooting
            }
            else
            {
                Debug.Log("No Shooter");
            }

            // Destroy the soldier after 5 seconds
            Destroy(gameObject, 5f);
        }
    }

}
