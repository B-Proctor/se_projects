using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int enemyScore = 0;

    public AudioClip[] paddleHitSounds; // Array of paddle hit sounds
    public AudioSource musicSource; // Reference to the music audio source
    public TextMeshProUGUI playerScoreText; // Reference to TextMeshPro for player score
    public TextMeshProUGUI enemyScoreText; // Reference to TextMeshPro for enemy score

    private AudioSource audioSource;
    private int totalHits; // Total paddle hits for determining pitch

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI(); // Initialize score display
        ResetMusicPitch();
    }

    public void PlayRandomHitSound()
    {
        if (paddleHitSounds.Length > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, paddleHitSounds.Length);
            audioSource.PlayOneShot(paddleHitSounds[randomIndex]);
        }
    }

    public void AdjustMusicPitch()
    {
        if (musicSource != null)
        {
            float newPitch = 1.0f + totalHits * 0.05f; // Increment pitch by 0.05 per hit
            musicSource.pitch = Mathf.Min(newPitch, 2.0f); // Cap pitch at 2.0
        }
    }

    public void ResetMusicPitch()
    {
        totalHits = 0; // Reset hit counter
        StartCoroutine(LowerPitchOverTime());
    }

    public void IncrementHitCounter()
    {
        totalHits++;
        AdjustMusicPitch();
    }

    public void AddScore(int scorer)
    {
        if (scorer == 1)
        {
            playerScore++;
        }
        else if (scorer == 2)
        {
            enemyScore++;
        }

        ResetMusicPitch(); // Start dynamic pitch lowering
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        playerScoreText.text = playerScore < 10 ? $"0{playerScore}" : playerScore.ToString();
        enemyScoreText.text = enemyScore < 10 ? $"0{enemyScore}" : enemyScore.ToString();
    }

    private IEnumerator LowerPitchOverTime()
    {
        float startPitch = musicSource.pitch;
        float duration = 3.0f; // Match the ball reset delay
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.pitch = Mathf.Lerp(startPitch, 1.0f, elapsed / duration);
            yield return null;
        }

        musicSource.pitch = 1.0f; // Ensure exact reset
    }

    public void ResetBall()
    {
        Ball ballScript = FindObjectOfType<Ball>();
        ballScript.transform.position = Vector3.zero;
        ballScript.LaunchBall();
    }
}
