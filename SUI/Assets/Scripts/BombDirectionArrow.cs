using UnityEngine;

public class BombDirectionArrow : MonoBehaviour
{
    public Transform cameraTransform;
    public BombSpawnerMan bombSpawner;

    [Header("Settings")]
    public float heightOffset = 0.5f;
    public float floatSpeed = 3f;
    public float floatAmount = 0.1f;

    [Header("Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float scaleMultiplier = 0.2f;

    void Update()
    {
        if (bombSpawner == null || cameraTransform == null) return;

        Vector3 bombPos = bombSpawner.GetBombPosition();

        // Floating effect
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        // Position arrow above bomb
        transform.position = bombPos + Vector3.up * (heightOffset + floatOffset);

        // Face the player (horizontal only)
        Vector3 lookDir = cameraTransform.position - transform.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Scale based on distance
        float dist = Vector3.Distance(cameraTransform.position, bombPos);
        float scale = Mathf.Clamp(dist * scaleMultiplier, minScale, maxScale);
        transform.localScale = Vector3.one * scale;

        // Hide when bomb is revealed
        if (bombSpawner.IsBombRevealed())
        {
            gameObject.SetActive(false);
        }
    }
}