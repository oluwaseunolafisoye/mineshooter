using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] GameObject robotExplosionVFX;
    [SerializeField] Vector3 vfxOffset = new Vector3(0, 1f, 0);
    [SerializeField] int health = 3;

    int currentHealth;
    void Awake()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(robotExplosionVFX, transform.position + vfxOffset, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SelfDestruct()
    {
        Die();
    }
}
