using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TMP_Text finalTimeText;
    public TMP_InputField nameInput;

    public string mainMenuSceneName = "StartMenu";

    void Start()
    {
        if (nameInput == null)
            nameInput = FindObjectOfType<TMP_InputField>();

        // 1. Grab all three phase times
        float totalTime = GameTimer.GetPhaseDuration("TotalGame");
        float findTime = GameTimer.GetPhaseDuration("FindPhase");
        float defuseTime = GameTimer.GetPhaseDuration("DefusePhase");

        // 2. Format them into a single string with line breaks (\n)
        if (finalTimeText != null)
        {
            finalTimeText.text = 
                "Total: " + totalTime.ToString("F2") + " seconds\n" +
                "Find: " + findTime.ToString("F2") + " seconds\n" +
                "Defuse: " + defuseTime.ToString("F2") + " seconds";
        }

        Debug.Log("ENDMENU START - TotalTime: " + totalTime);
        Debug.Log("ENDMENU START - NameInput: " + nameInput);
    }

    public void SaveScore()
    {
        if (nameInput == null)
        {
            nameInput = FindObjectOfType<TMP_InputField>();
        }

        string playerName = "Player";

        if (nameInput != null)
        {
            playerName = nameInput.text;

            if (string.IsNullOrWhiteSpace(playerName))
                playerName = "Player";
        }

        // IMPORTANT: Grab all three times to save!
        float totalTimeToSave = GameTimer.GetPhaseDuration("TotalGame");
        float findTimeToSave = GameTimer.GetPhaseDuration("FindPhase");
        float defuseTimeToSave = GameTimer.GetPhaseDuration("DefusePhase");

        // Send all of them to the database
        ScoreDatabase.SaveScore(playerName, totalTimeToSave, findTimeToSave, defuseTimeToSave);

        Debug.Log("SAVED SCORE: " + playerName + " - Total: " + totalTimeToSave.ToString("F2"));
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}