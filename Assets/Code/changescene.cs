using UnityEngine;
using UnityEngine.SceneManagement;

public class changescene : MonoBehaviour
{
    public string sceneName;
    public Canvas gameOverCanvas; // Reference to the Canvas that needs to be checked

    private void Awake()
    {
        if (gameOverCanvas == null)
        {
            Debug.LogError("Game Over Canvas is not assigned!");
        }
    }

    public void OnGomenu()
    {
        Debug.Log("Going to Main Menu");
        SceneManager.LoadScene("Start Game");
    }

    public void OnRestart()
    {
        Debug.Log("Restart button pressed");

        if (gameOverCanvas != null && gameOverCanvas.gameObject.activeInHierarchy)
        {
            Debug.Log("Canvas is active. Restarting the level.");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        else
        {
            Debug.LogWarning("Restart action ignored because the canvas is not active.");
        }
    }
}
