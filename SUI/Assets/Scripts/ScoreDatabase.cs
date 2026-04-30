using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public float totalTime;
    public float findTime;
    public float defuseTime;
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

    // 1. Update this to accept all three times!
    public static void SaveScore(string playerName, float totalTime, float findTime, float defuseTime)
    {
        ScoreData data = LoadScores();

        ScoreEntry entry = new ScoreEntry();
        entry.playerName = playerName;
        
        // 2. Assign the new variables
        entry.totalTime = totalTime;
        entry.findTime = findTime;
        entry.defuseTime = defuseTime;

        data.scores.Add(entry);

        // 3. Sort by totalTime instead of the old 'time' variable
        data.scores.Sort((a, b) => a.totalTime.CompareTo(b.totalTime));

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