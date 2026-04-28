using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SequenceMiniGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject gamePanel;
    public Button[] colorButtons;
    public TMP_Text instructionText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;

    [Header("Settings")]
    public int sequenceLength = 5;
    public float flashTime = 0.45f;
    public float pauseBetweenFlashes = 0.25f;
    public string nextSceneName = "MainScene";

    private List<int> sequence = new List<int>();
    private int playerIndex = 0;
    private bool acceptingInput = false;

    void Start()
    {
        gamePanel.SetActive(false);

        for (int i = 0; i < colorButtons.Length; i++)
        {
            int buttonIndex = i;
            colorButtons[i].onClick.AddListener(() => PlayerPressed(buttonIndex));
        }
    }

    public void StartGame()
    {
        gamePanel.SetActive(true);

        sequence.Clear();
        playerIndex = 0;
        acceptingInput = false;

        if (instructionText != null)
            instructionText.text = "Watch the sequence";

        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(Random.Range(0, colorButtons.Length));
        }

        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (int index in sequence)
        {
            yield return FlashButton(index);
            yield return new WaitForSeconds(pauseBetweenFlashes);
        }

        acceptingInput = true;

        if (instructionText != null)
            instructionText.text = "Press the correct sequence";
    }

    IEnumerator FlashButton(int index)
    {
        Image image = colorButtons[index].GetComponent<Image>();

        Color originalColor = image.color;
        Vector3 originalScale = colorButtons[index].transform.localScale;

        image.color = Color.white;
        colorButtons[index].transform.localScale = originalScale * 1.15f;

        PlaySound();

        yield return new WaitForSeconds(flashTime);

        image.color = originalColor;
        colorButtons[index].transform.localScale = originalScale;
    }

    void PlayerPressed(int index)
    {
        if (!acceptingInput) return;

        StartCoroutine(FlashButton(index));

        if (index == sequence[playerIndex])
        {
            playerIndex++;

            if (instructionText != null)
                instructionText.text = "Correct";

            if (playerIndex >= sequence.Count)
            {
                Debug.Log("Sequence mini-game complete!");

                if (instructionText != null)
                    instructionText.text = "Mini-game complete";

                StartCoroutine(CloseAfterDelay());
            }
        }
        else
        {
            Debug.Log("Wrong sequence! Try again.");

            if (instructionText != null)
                instructionText.text = "Wrong, try again";

            StartCoroutine(RestartAfterDelay());
        }
    }

    IEnumerator RestartAfterDelay()
    {
        acceptingInput = false;
        yield return new WaitForSeconds(1f);
        StartGame();
    }

    IEnumerator CloseAfterDelay()
    {
        acceptingInput = false;
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        SceneManager.LoadScene(nextSceneName);
    }

    void PlaySound()
    {
        if (audioSource != null && buttonSound != null)
        {
            audioSource.PlayOneShot(buttonSound);
        }
    }
}