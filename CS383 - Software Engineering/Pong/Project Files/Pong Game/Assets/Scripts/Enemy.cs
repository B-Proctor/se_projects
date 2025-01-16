using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speedEasy = 3f; // Easy speed
    public float speedMedium = 5f; // Medium speed
    public float speedImpossible = 10f; // Impossible speed

    public float reactionTimeEasy = 0.4f; // Easy reaction time
    public float reactionTimeMedium = 0.2f; // Medium reaction time
    public float reactionTimeImpossible = 0.05f; // Impossible reaction time

    public float errorMarginEasy = 0.5f; // Easy error margin
    public float errorMarginMedium = 0.3f; // Medium error margin
    public float errorMarginImpossible = 0.0f; // Impossible has no error

    private Transform ball;
    private Vector3 paddlePosition;
    private float targetY;
    private float timeSinceLastUpdate;

    private float currentSpeed;
    private float currentReactionTime;
    private float currentErrorMargin;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
        targetY = transform.position.y;

        SetDifficulty(PlayerPrefs.GetInt("Difficulty", 1)); // Load saved difficulty (default: Medium)
    }

    void Update()
    {
        if (ball == null) return;

        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= currentReactionTime)
        {
            targetY = ball.position.y + Random.Range(-currentErrorMargin, currentErrorMargin);
            timeSinceLastUpdate = 0f;
        }

        // Move paddle towards the target Y position
        paddlePosition = transform.position;

        if (targetY > paddlePosition.y)
        {
            paddlePosition.y += currentSpeed * Time.deltaTime;
        }
        else if (targetY < paddlePosition.y)
        {
            paddlePosition.y -= currentSpeed * Time.deltaTime;
        }

        paddlePosition.y = Mathf.Clamp(paddlePosition.y, -4.5f, 4.5f); // Clamp within boundaries
        transform.position = paddlePosition;
    }

    public void SetDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0: // Easy
                currentSpeed = speedEasy;
                currentReactionTime = reactionTimeEasy;
                currentErrorMargin = errorMarginEasy;
                break;
            case 1: // Medium
                currentSpeed = speedMedium;
                currentReactionTime = reactionTimeMedium;
                currentErrorMargin = errorMarginMedium;
                break;
            case 2: // Impossible
                currentSpeed = speedImpossible;
                currentReactionTime = reactionTimeImpossible;
                currentErrorMargin = errorMarginImpossible;
                break;
        }
    }
}
