using UnityEngine;
using UnityEngine.EventSystems;

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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // Check if touch is over UI first
                if (!IsPointerOverUI(touch.position))
                {
                    TryClick(touch.position);
                }
            }
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI(Input.mousePosition))
            {
                TryClick(Input.mousePosition);
            }
        }
#endif
    }

    void TryClick(Vector2 screenPosition)
    {
        if (sequenceMiniGame == null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
            {
                Debug.Log("Bomb clicked!");
                sequenceMiniGame.StartGame();
            }
        }
    }

    bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;
        
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        
        return results.Count > 0;
    }
}