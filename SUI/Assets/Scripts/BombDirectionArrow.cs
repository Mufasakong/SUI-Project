using UnityEngine;

public class BombDirectionArrow : MonoBehaviour
{
    public Transform cameraTransform;
    public BombSpawnerMan bombSpawner;

    [Header("Settings")]
    public float heightOffset = 0.25f;
    public float forwardOffset = 0.6f;
    public float rotationSpeed = 5f;
    public float hideDistance = 0.5f;

    void Update()
    {
        if (bombSpawner == null || cameraTransform == null) return;

        Vector3 bombPos = bombSpawner.GetBombPosition();

        // Keep arrow in front of camera (top of screen)
        transform.position = cameraTransform.position 
                           + cameraTransform.forward * forwardOffset
                           + Vector3.up * heightOffset;

        // Direction to bomb (horizontal only)
        Vector3 direction = bombPos - cameraTransform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Quaternion flatRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        // Smooth rotation
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            flatRotation,
            Time.deltaTime * rotationSpeed
        );

        // Hide when very close to bomb
        float dist = Vector3.Distance(cameraTransform.position, bombPos);
        gameObject.SetActive(dist > hideDistance);
    }
}