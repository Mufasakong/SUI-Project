using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public float time;
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scores = new List<ScoreEntry>();
}

public static class ScoreDatabase
{
    private static string FilePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "scores.json");
        }
    }

    public static void SaveScore(string playerName, float time)
    {
        ScoreData data = LoadScores();

        ScoreEntry entry = new ScoreEntry();
        entry.playerName = playerName;
        entry.time = time;

        data.scores.Add(entry);

        data.scores.Sort((a, b) => a.time.CompareTo(b.time));

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);

        Debug.Log("Score saved to: " + FilePath);
    }

    public static ScoreData LoadScores()
    {
        if (!File.Exists(FilePath))
            return new ScoreData();

        string json = File.ReadAllText(FilePath);
        return JsonUtility.FromJson<ScoreData>(json);
    }
}