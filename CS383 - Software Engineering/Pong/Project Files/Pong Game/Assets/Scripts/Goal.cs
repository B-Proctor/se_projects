using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int scorer; // 1 for player, 2 for enemy
    public AudioClip goalSound; // Sound to play when a goal is scored
    private GameManager gameManager;
    private AudioSource audioSource;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            PlayGoalSound();
            gameManager.AddScore(scorer); // Update the score
            StartCoroutine(HandleGoal(other.gameObject));
        }
    }

    void PlayGoalSound()
    {
        if (audioSource != null && goalSound != null)
        {
            audioSource.PlayOneShot(goalSound);
        }
    }

    IEnumerator HandleGoal(GameObject ball)
    {
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop the ball
        ball.transform.position = Vector3.zero; // Reset ball position

        yield return new WaitForSeconds(3); // Wait 3 seconds

        gameManager.ResetBall(); // Launch the ball again
    }
}
