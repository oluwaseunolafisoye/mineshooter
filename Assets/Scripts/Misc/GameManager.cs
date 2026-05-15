using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] GameObject levelCleared;
    [SerializeField] GameObject aimOverlay;

    int enemiesLeft = 0;
    const string ENEMIES_LEFT_STRING = "ENEMIES: ";

    public void UpdateEnemiesLeft(int count)
    {
        enemiesLeft += count;
        enemiesLeftText.text = ENEMIES_LEFT_STRING + enemiesLeft.ToString();
        if (enemiesLeft <= 0)
        {
            levelCleared.SetActive(true);

            StarterAssetsInputs starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            starterAssetsInputs.SetCursorState(false);
            starterAssetsInputs.enabled = false;

            aimOverlay.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void QuitGame()
    {
        Debug.LogWarning("Doesn't work in editor. Build the game to test quitting.");
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0); // loop back to main menu or level 1
    }
}
