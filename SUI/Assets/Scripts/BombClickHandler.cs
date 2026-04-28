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
        if (Input.GetMouseButtonDown(0))
        {
            TryClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryClick(Input.GetTouch(0).position);
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