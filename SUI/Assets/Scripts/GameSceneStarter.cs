using UnityEngine;

public class GameSceneStarter : MonoBehaviour
{
    void Start()
    {
        // Replaced StartTimer() with the new Phase system
        GameTimer.StartPhase("TotalGame");
        Debug.Log("SCENE START: Timer started");
    }
}