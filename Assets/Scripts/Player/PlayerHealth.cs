using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int health = 5;
    [SerializeField] CinemachineVirtualCamera deathCamera;
    // [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] heartImages;

    int currentHealth;
    int gameOverVirtualCameraPriority = 20;
    void Awake()
    {
        currentHealth = health;
        ChangeShieldUIBars();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        ChangeShieldUIBars();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ChangeShieldUIBars();
        if (currentHealth <= 0)
        {
            // weaponCamera.parent = null;
            deathCamera.Priority = gameOverVirtualCameraPriority;
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void ChangeShieldUIBars()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].gameObject.SetActive(true);
            }
            else
            {
                heartImages[i].gameObject.SetActive(false);
            }
        }
    }

}
