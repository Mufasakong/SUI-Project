using UnityEngine;
using UnityEngine.InputSystem;

public class TestGameStarter : MonoBehaviour
{
    public SequenceMiniGame sequenceMiniGame;
    public Transform cameraTransform;

    [Header("Fake Bomb Settings")]
    public GameObject fakeBombPrefab;
    public float distanceFromCamera = 2f;
    public float heightOffset = -0.3f;

    private Transform currentFakeBomb;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.bKey.wasPressedThisFrame)
        {
            StartTestGame();
        }
    }

    void StartTestGame()
    {
        SpawnFakeBomb();

        if (sequenceMiniGame != null && cameraTransform != null && currentFakeBomb != null)
        {
            sequenceMiniGame.StartGame(cameraTransform, currentFakeBomb);
            Debug.Log("TEST: Mini-game started");
        }
        else
        {
            Debug.LogWarning("Missing references on TestGameStarter!");
        }
    }

    void SpawnFakeBomb()
    {
        if (fakeBombPrefab == null)
        {
            Debug.LogWarning("No fake bomb prefab assigned!");
            return;
        }

        // Remove old one if it exists
        if (currentFakeBomb != null)
        {
            Destroy(currentFakeBomb.gameObject);
        }

        // Flatten forward (like your real bomb logic)
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;

        if (forward.sqrMagnitude < 0.001f)
            forward = cameraTransform.forward;

        forward.Normalize();

        Vector3 spawnPos = cameraTransform.position + forward * distanceFromCamera;
        spawnPos.y += heightOffset;

        GameObject bomb = Instantiate(fakeBombPrefab, spawnPos, Quaternion.identity);
        currentFakeBomb = bomb.transform;
    }
}