using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndMenu : MonoBehaviour
{
    public TMP_Text finalTimeText;
    public TMP_InputField nameInput;

    public string mainMenuSceneName = "StartMenu";

    private float finalTime;

    void Start()
    {
        if (nameInput == null)
            nameInput = FindObjectOfType<TMP_InputField>();

        finalTime = GameTimer.GetFinalTime();

        if (finalTimeText != null)
            finalTimeText.text = "Your time: " + finalTime.ToString("F2") + " seconds";

        Debug.Log("ENDMENU START - FinalTime: " + finalTime);
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

        // IMPORTANT: get the latest time again when saving
        float timeToSave = GameTimer.GetFinalTime();

        ScoreDatabase.SaveScore(playerName, timeToSave);

        Debug.Log("SAVED SCORE: " + playerName + " - " + timeToSave.ToString("F2"));
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}