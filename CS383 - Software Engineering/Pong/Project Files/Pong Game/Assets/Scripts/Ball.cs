using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 10f; // Initial speed of the ball
    public float speedIncrement = 0.5f; // Amount to increase speed per hit
    public float maxSpeed = 20f; // Maximum allowed speed for the ball
    public float gradualSpeedIncrease = 0.05f; // Speed increase over time

    private Rigidbody2D rb;
    private GameManager gameManager;
    private ComboCounter comboCounter; // Reference to the combo counter
    private AudioSource audioSource;

    [SerializeField]
    private float currentSpeed; // Current speed of the ball, shown in the Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        comboCounter = FindObjectOfType<ComboCounter>(); // Find the ComboCounter in the scene
        audioSource = GetComponent<AudioSource>();
        LaunchBall();
        StartCoroutine(GraduallyIncreaseSpeed());
    }

    void Update()
    {
        currentSpeed = rb.velocity.magnitude; // Update current speed for debugging
    }

    public void LaunchBall()
    {
        int directionX = Random.Range(0, 2) == 0 ? -1 : 1;
        int directionY = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 launchDirection = new Vector2(directionX, directionY).normalized;
        rb.velocity = launchDirection * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            PlayHitSound();
            IncreaseSpeed();
            gameManager?.IncrementHitCounter(); // Increment hit counter for pitch adjustment
            comboCounter?.IncrementCombo(); // Increment the combo counter
        }
    }

    void IncreaseSpeed()
    {
        float newSpeed = rb.velocity.magnitude + speedIncrement;
        rb.velocity = rb.velocity.normalized * Mathf.Min(newSpeed, maxSpeed);
    }

    void PlayHitSound()
    {
        if (audioSource != null && gameManager != null)
        {
            AudioClip[] hitSounds = gameManager.paddleHitSounds;
            if (hitSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, hitSounds.Length);
                audioSource.PlayOneShot(hitSounds[randomIndex]);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            ResetOnGoal(); // Handle goal event
        }
    }

    void ResetOnGoal()
    {
        comboCounter?.ResetCombo(); // Reset the combo counter with fade-out
        rb.velocity = Vector2.zero; // Stop the ball
        transform.position = Vector3.zero; // Reset ball position
        gameManager?.ResetBall(); // Reset and relaunch the ball
    }

    IEnumerator GraduallyIncreaseSpeed()
    {
        while (true)
        {
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.velocity += rb.velocity.normalized * gradualSpeedIncrease * Time.deltaTime;
            }
            yield return null;
        }
    }
}
