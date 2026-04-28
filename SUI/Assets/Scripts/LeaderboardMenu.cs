using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderboardMenu : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public Transform contentParent;
    public GameObject scoreRowPrefab;

    [Header("Scenes")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        ShowScores();
    }

    void ShowScores()
    {
        ScoreData data = ScoreDatabase.LoadScores();

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (data.scores.Count == 0)
        {
            CreateRow("No scores yet.");
            return;
        }

        for (int i = 0; i < data.scores.Count; i++)
        {
            ScoreEntry entry = data.scores[i];

            string rowText =
                (i + 1) + ". " +
                entry.playerName +
                " - " +
                entry.time.ToString("F2") +
                "s";

            CreateRow(rowText);
        }
    }

    void CreateRow(string text)
    {
        GameObject row = Instantiate(scoreRowPrefab, contentParent);

        TMP_Text rowText = row.GetComponent<TMP_Text>();

        if (rowText != null)
        {
            rowText.text = text;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}