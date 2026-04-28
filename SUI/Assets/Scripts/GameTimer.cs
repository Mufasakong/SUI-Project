using UnityEngine;

public class GameTimer
{
    public static void StartTimer()
    {
        float startTime = Time.realtimeSinceStartup;

        PlayerPrefs.SetFloat("StartTime", startTime);
        PlayerPrefs.SetFloat("FinalTime", 0f);
        PlayerPrefs.Save();

        Debug.Log("TIMER STARTED: " + startTime);
    }

    public static void StopTimer()
    {
        float startTime = PlayerPrefs.GetFloat("StartTime", -1f);
        Debug.Log("LOADED START TIME: " + startTime);

        if (startTime < 0f)
        {
            Debug.LogWarning("No StartTime found, cannot calculate final time.");
            PlayerPrefs.SetFloat("FinalTime", 0f);
        }
        else
        {
            float finalTime = Time.realtimeSinceStartup - startTime;

            PlayerPrefs.SetFloat("FinalTime", finalTime);
            PlayerPrefs.Save();

            Debug.Log("TIMER STOPPED: " + finalTime);
        }
    }

    public static float GetFinalTime()
    {
        float finalTime = PlayerPrefs.GetFloat("FinalTime", 0f);
        Debug.Log("FINAL TIME LOADED: " + finalTime);
        return finalTime;
    }
}