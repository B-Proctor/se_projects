using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int scorer; 
    public AudioClip goalSound; 
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
            gameManager.AddScore(scorer); 
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
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        ball.transform.position = Vector3.zero; 

        yield return new WaitForSeconds(3); 

        gameManager.ResetBall(); 
    }
}
