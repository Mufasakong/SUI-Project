using UnityEngine;

public class GameTimer
{
    // Pass in a name for the phase (e.g., "FindBomb", "DefuseMinigame")
    public static void StartPhase(string phaseName)
    {
        float startTime = Time.realtimeSinceStartup;
        PlayerPrefs.SetFloat(phaseName + "_StartTime", startTime);
        PlayerPrefs.SetFloat(phaseName + "_Duration", 0f);
        PlayerPrefs.Save();

        Debug.Log($"TIMER STARTED [{phaseName}]: {startTime}");
    }

    // Stop the timer for that specific phase
    public static void StopPhase(string phaseName)
    {
        float startTime = PlayerPrefs.GetFloat(phaseName + "_StartTime", -1f);

        if (startTime < 0f)
        {
            Debug.LogWarning($"No StartTime found for phase '{phaseName}'. Cannot calculate duration.");
            PlayerPrefs.SetFloat(phaseName + "_Duration", 0f);
        }
        else
        {
            float duration = Time.realtimeSinceStartup - startTime;
            PlayerPrefs.SetFloat(phaseName + "_Duration", duration);
            PlayerPrefs.Save();

            Debug.Log($"TIMER STOPPED [{phaseName}]: Took {duration} seconds.");
        }
    }

    // Get the final duration of a phase (useful for your end-game TMP screen)
    public static float GetPhaseDuration(string phaseName)
    {
        return PlayerPrefs.GetFloat(phaseName + "_Duration", 0f);
    }

    // Optional: Format the time for TextMeshPro (e.g., "01:23")
    public static string GetFormattedTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}