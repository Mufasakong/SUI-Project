using UnityEngine;

public class GameSceneStarter : MonoBehaviour
{
    void Start()
    {
        GameTimer.StartTimer();
        Debug.Log("SCENE START: Timer started");
    }
}