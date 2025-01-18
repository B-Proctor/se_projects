using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speedEasy = 3f;
    public float speedMedium = 5f;
    public float speedImpossible = 10f;

    public float reactionTimeEasy = 0.4f;
    public float reactionTimeMedium = 0.2f;
    public float reactionTimeImpossible = 0.05f;

    public float errorMarginEasy = 0.5f;
    public float errorMarginMedium = 0.3f;
    public float errorMarginImpossible = 0.0f;

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

        SetDifficulty(PlayerPrefs.GetInt("Difficulty", 1));
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

        paddlePosition = transform.position;

        if (targetY > paddlePosition.y)
        {
            paddlePosition.y += currentSpeed * Time.deltaTime;
        }
        else if (targetY < paddlePosition.y)
        {
            paddlePosition.y -= currentSpeed * Time.deltaTime;
        }

        paddlePosition.y = Mathf.Clamp(paddlePosition.y, -4.5f, 4.5f);
        transform.position = paddlePosition;
    }

    public void SetDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                currentSpeed = speedEasy;
                currentReactionTime = reactionTimeEasy;
                currentErrorMargin = errorMarginEasy;
                break;
            case 1:
                currentSpeed = speedMedium;
                currentReactionTime = reactionTimeMedium;
                currentErrorMargin = errorMarginMedium;
                break;
            case 2:
                currentSpeed = speedImpossible;
                currentReactionTime = reactionTimeImpossible;
                currentErrorMargin = errorMarginImpossible;
                break;
        }
    }
}
