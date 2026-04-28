using UnityEngine;

public class BombClickHandler : MonoBehaviour
{
    private SequenceMiniGame sequenceMiniGame;

    void Start()
    {
        sequenceMiniGame = FindObjectOfType<SequenceMiniGame>();

        if (sequenceMiniGame == null)
        {
            Debug.LogError("SequenceMiniGame not found in scene!");
        }
    }

    void Update()
        {
        // 1. Check for real mobile screen touches first
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryClick(Input.GetTouch(0).position);
        }
        // 2. If no touch happened, check for a mouse click (for Unity Editor testing)
        else if (Input.GetMouseButtonDown(0))
        {
            TryClick(Input.mousePosition);
        }
    }

    void TryClick(Vector2 screenPosition)
    {
        if (sequenceMiniGame == null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                sequenceMiniGame.StartGame();
            }
        }
    }
}