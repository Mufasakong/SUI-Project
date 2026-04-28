using UnityEngine;
using System.Collections;
using TMPro;

public class BombSpawnerMan : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public GameObject bombPrefab;
    public AudioSource audioSource;
    public TMP_Text holdText;

    [Header("Spawn Settings")]
    public float minSpawnRadius = 1.0f;
    public float maxSpawnRadius = 2.0f;

    [Header("Find / Hold Settings")]
    // Increased this so it's larger than the maxSpawnRadius. 
    // This allows the player to trigger it just by turning around!
    public float revealDistance = 2.5f; 
    public float aimThreshold = 0.95f;
    public float requiredHoldTime = 3f;

    [Header("Beep Settings")]
    public float farBeepInterval = 1.2f;
    public float mediumBeepInterval = 0.5f;
    public float closeBeepInterval = 0.15f;

    private Vector3 bombPosition;
    private GameObject bombInstance;

    private bool bombSpawned = false;
    private bool bombRevealed = false;

    private float beepTimer = 0f;
    private float beepInterval = 1.2f;
    private float holdTimer = 0f;

    private float previousDistance = -1f;

    void Start()
    {
        if (holdText != null)
            holdText.text = "";

        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SpawnBomb();
    }

    void SpawnBomb()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform missing!");
            return;
        }

        Vector3 randomDirection = Random.onUnitSphere;
        float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

        bombPosition = cameraTransform.position + randomDirection * spawnDistance;

        bombSpawned = true;
        bombRevealed = false;
        holdTimer = 0f;
        previousDistance = -1f;

        Debug.Log("Hidden bomb spawned at: " + bombPosition);
    }

    void Update()
    {
        if (!bombSpawned || bombRevealed) return;

        float distanceToBomb = Vector3.Distance(cameraTransform.position, bombPosition);
        Vector3 directionToBomb = (bombPosition - cameraTransform.position).normalized;
        float aimDot = Vector3.Dot(cameraTransform.forward, directionToBomb);

        if (previousDistance >= 0f)
        {
            if (distanceToBomb < previousDistance - 0.02f)
            {
                // Debug.Log("Getting closer...");
            }
            else if (distanceToBomb > previousDistance + 0.02f)
            {
                // Debug.Log("Getting farther...");
            }
        }

        previousDistance = distanceToBomb;

        bool closeEnough = distanceToBomb <= revealDistance;
        bool lookingAtBomb = aimDot >= aimThreshold;

        if (closeEnough && lookingAtBomb)
        {
            holdTimer += Time.deltaTime;
            beepInterval = closeBeepInterval;

            if (holdText != null)
            {
                float remaining = Mathf.Max(0f, requiredHoldTime - holdTimer);
                holdText.text = "Hold steady... digging up bomb (" + Mathf.Ceil(remaining) + ")";
            }

            if (holdTimer >= requiredHoldTime)
            {
                if (holdText != null)
                    holdText.text = "";

                RevealBomb();
                return;
            }
        }
        else
        {
            // AR FIX: Slowly drain the timer instead of an instant wipe.
            // Multiplying by 1.5 makes it drain a bit faster than it fills.
            holdTimer = Mathf.Max(0f, holdTimer - (Time.deltaTime * 1.5f));

            if (holdText != null)
            {
                if (holdTimer > 0f)
                {
                    // Let them know they are losing the lock
                    holdText.text = "Tracking lost! Re-center..."; 
                }
                else
                {
                    holdText.text = ""; // Clear only when fully drained
                }
            }

            if (distanceToBomb < 0.8f || aimDot > 0.85f)
            {
                beepInterval = mediumBeepInterval;
            }
            else
            {
                beepInterval = farBeepInterval;
            }
        }

        beepTimer += Time.deltaTime;

        if (beepTimer >= beepInterval)
        {
            PlayBeep();
            beepTimer = 0f;
        }
    }

    void PlayBeep()
    {
        if (audioSource == null) return;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    void RevealBomb()
    {
        bombRevealed = true;

        if (bombPrefab == null)
        {
            Debug.LogError("Bomb Prefab missing!");
            return;
        }

        float revealDistanceFromPlayer = 3f;

        // AR FIX: Flatten the forward vector so the bomb doesn't spawn deep underground
        Vector3 flatForward = cameraTransform.forward;
        flatForward.y = 0; 
        
        // Prevent a rare error if the player is looking EXACTLY straight down
        if (flatForward.sqrMagnitude > 0.001f) 
        {
            flatForward.Normalize();
        }
        else 
        {
            flatForward = cameraTransform.up; 
        }

        Vector3 revealPosition = cameraTransform.position + (flatForward * revealDistanceFromPlayer);
        
        // Optional: Drop the bomb down half a meter so it sits closer to the floor
        revealPosition.y -= 0.5f; 

        bombInstance = Instantiate(
            bombPrefab,
            revealPosition,
            Quaternion.identity
        );

        Debug.Log("THE BOMB HAS BEEN REVEALED! OH NO!");
    }

    public bool IsBombRevealed()
    {
        return bombRevealed;
    }

    public Vector3 GetBombPosition()
    {
        return bombPosition;
    }
}