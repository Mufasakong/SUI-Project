using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMan : MonoBehaviour
{
    public string gameSceneName = "MainScene";
    public string leaderboardSceneName = "Leaderboard";

    public void StartGame()
    {
        SceneManager.LoadScene("BombScene");
    }

    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void OpenEndMenu()
    {
        SceneManager.LoadScene("EndMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}