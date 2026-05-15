using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] int health = 5;
    [SerializeField] CinemachineVirtualCamera deathCamera;
    // [SerializeField] Transform weaponCamera;
    [SerializeField] Image[] heartImages;
    [SerializeField] GameObject gameOverScreen;

    int currentHealth;
    int gameOverVirtualCameraPriority = 20;
    void Awake()
    {
        currentHealth = health;
        ChangeHealthUIBars();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, health);
        ChangeHealthUIBars();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ChangeHealthUIBars();
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // weaponCamera.parent = null;
        deathCamera.Priority = gameOverVirtualCameraPriority;
        gameOverScreen.SetActive(true);
        StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        starterAssetsInputs.SetCursorState(false);
        starterAssetsInputs.enabled = false;

        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void ChangeHealthUIBars()
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
