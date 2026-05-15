using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject robotExplosionVFX;
    [SerializeField] Vector3 vfxOffset = new Vector3(0, 1f, 0);
    [SerializeField] int health = 3;

    GameManager gameManager;

    int currentHealth;
    void Awake()
    {
        currentHealth = health;
    }

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.UpdateEnemiesLeft(1);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameManager.UpdateEnemiesLeft(-1);
            Die();
        }
    }

    void Die()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        Vector3 spawnPos = renderer ? renderer.bounds.center + vfxOffset : transform.position + vfxOffset;
        Instantiate(robotExplosionVFX, spawnPos, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SelfDestruct()
    {
        if (gameManager) gameManager.UpdateEnemiesLeft(-1);
        Die();
    }
}
