using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMan : MonoBehaviour
{
    public void LoadNextScene()
    {
        // Gets the current scene's index and loads the next one in line
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}