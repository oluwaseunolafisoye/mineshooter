using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    enum State { Patrol, Chase, Attack }

    [Header("Combat")]
    [SerializeField] int damage = 1;
    [SerializeField] float fireRate = .5f;
    [SerializeField] float shootRange = 5f;

    [Header("Detection")]
    [SerializeField] float sightRange = 12f;
    [SerializeField] float fieldOfViewAngle = 110f;

    [Header("Patrol")]
    [SerializeField] float patrolRadius = 8f;
    [SerializeField] float waypointReachedDistance = 1f;

    FirstPersonController player;
    PlayerHealth playerHealth;
    NavMeshAgent agent;
    State currentState;
    Vector3 spawnPoint;
    Vector3 patrolTarget;
    Vector3 chaseOffset;
    float nextShootTime;
    const string PLAYER_STRING = "Player";

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(0, 100);
    }

    void Start()
    {
        player = FindFirstObjectByType<FirstPersonController>();
        playerHealth = player?.GetComponent<PlayerHealth>();
        spawnPoint = transform.position;
        currentState = State.Patrol;
        chaseOffset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        PickNewPatrolTarget();
    }

    void Update()
    {
        if (!player) return;

        switch (currentState)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
            case State.Attack: UpdateAttack(); break;
        }
    }

    void UpdatePatrol()
    {
        if (CanSeePlayer())
        {
            currentState = State.Chase;
            return;
        }

        if (agent.remainingDistance < waypointReachedDistance)
            PickNewPatrolTarget();
    }

    void PickNewPatrolTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        Vector3 candidate = spawnPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            patrolTarget = hit.position;
        else
            patrolTarget = spawnPoint;

        agent.SetDestination(patrolTarget);
    }

    // --- Chase ---

    void UpdateChase()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= shootRange)
        {
            currentState = State.Attack;
            agent.ResetPath();
            return;
        }

        if (!CanSeePlayer() && distanceToPlayer > sightRange)
        {
            currentState = State.Patrol;
            PickNewPatrolTarget();
            return;
        }

        agent.SetDestination(player.transform.position + chaseOffset);
    }

    // --- Attack ---

    void UpdateAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > shootRange)
        {
            currentState = State.Chase;
            return;
        }

        // Face the player while attacking
        Vector3 lookDirection = player.transform.position - transform.position;
        lookDirection.y = 0f;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 8f);

        TryShoot();
    }

    void TryShoot()
    {
        if (Time.time < nextShootTime) return;

        Vector3 directionToPlayer = player.transform.position - transform.position;
        nextShootTime = Time.time + fireRate;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, shootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.GetComponentInParent<PlayerHealth>() != null)
                playerHealth?.TakeDamage(damage);
        }
    }

    // --- Perception ---

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        if (distance > sightRange) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfViewAngle / 2f) return false;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, sightRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            return hit.collider.GetComponentInParent<PlayerHealth>() != null;

        return false;
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
