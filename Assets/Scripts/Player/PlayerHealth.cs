using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] int health = 5;

    int currentHealth;
    void Awake()
    {
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(damage + " damage dealt. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
