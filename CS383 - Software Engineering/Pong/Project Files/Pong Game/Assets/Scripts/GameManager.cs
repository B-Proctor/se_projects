using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int playerScore = 0;
    public int enemyScore = 0;

    public AudioClip[] paddleHitSounds;
    public AudioSource musicSource;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI enemyScoreText;

    private AudioSource audioSource;
    private int totalHits;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreUI();
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
            float newPitch = 1.0f + totalHits * 0.05f;
            musicSource.pitch = Mathf.Min(newPitch, 2.0f);
        }
    }

    public void ResetMusicPitch()
    {
        totalHits = 0;
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

        ResetMusicPitch();
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
        float duration = 3.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.pitch = Mathf.Lerp(startPitch, 1.0f, elapsed / duration);
            yield return null;
        }

        musicSource.pitch = 1.0f;
    }

    public void ResetBall()
    {
        Ball ballScript = FindObjectOfType<Ball>();
        ballScript.transform.position = Vector3.zero;
        ballScript.LaunchBall();
    }
}
