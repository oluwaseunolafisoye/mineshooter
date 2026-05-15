using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float shootRange = 5f;

    FirstPersonController player;
    PlayerHealth playerHealth;
    NavMeshAgent agent;
    float nextShootTime;
    const string PLAYER_STRING = "Player";

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
        playerHealth = player?.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (!player) return;
        agent.SetDestination(player.transform.position);
        TryShoot();
    }

    void TryShoot()
    {
        if (Time.time < nextShootTime) return;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        if (directionToPlayer.magnitude > shootRange) return;

        nextShootTime = Time.time + fireRate;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, shootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.GetComponentInParent<PlayerHealth>() != null)
            {
                playerHealth?.TakeDamage(damage);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_STRING))
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
            enemyHealth?.SelfDestruct();
        }
    }
}
