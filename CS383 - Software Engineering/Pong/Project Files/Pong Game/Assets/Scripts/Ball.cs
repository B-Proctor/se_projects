using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedIncrement = 0.5f;
    public float maxSpeed = 20f;
    public float gradualSpeedIncrease = 0.05f;

    public GameObject impactEffectPrefab;

    private Rigidbody2D rb;
    private GameManager gameManager;
    private ComboCounter comboCounter;
    private AudioSource audioSource;

    [SerializeField]
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        comboCounter = FindObjectOfType<ComboCounter>();
        audioSource = GetComponent<AudioSource>();
        LaunchBall();
        StartCoroutine(GraduallyIncreaseSpeed());
    }

    void Update()
    {
        currentSpeed = rb.velocity.magnitude;
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
        SpawnImpactEffect(collision);

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            PlayHitSound();
            IncreaseSpeed();
            gameManager?.IncrementHitCounter();
            comboCounter?.IncrementCombo();
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

    void SpawnImpactEffect(Collision2D collision)
    {
        if (impactEffectPrefab != null)
        {
            Vector3 impactPoint = collision.contacts[0].point;
            Vector3 normal = collision.contacts[0].normal;

            GameObject impactEffect = Instantiate(impactEffectPrefab, impactPoint, Quaternion.LookRotation(Vector3.forward, normal));

            Destroy(impactEffect, 1.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goal"))
        {
            ResetOnGoal();
        }
    }

    void ResetOnGoal()
    {
        comboCounter?.ResetCombo();
        rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        gameManager?.ResetBall();
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
