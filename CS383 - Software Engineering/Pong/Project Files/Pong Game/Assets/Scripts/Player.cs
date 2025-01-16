using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Speed of the paddle movement
    public float boundaryY = 4.5f; // Limits for paddle movement along the Y-axis

    public bool isPlayer1; // True if this is Player 1
    private bool isTwoPlayerMode; // True if 2-Player Mode is active

    void Update()
    {
        // Check the current game mode (2-Player or 1-Player)
        isTwoPlayerMode = PlayerPrefs.GetInt("TwoPlayerMode", 0) == 1;

        if (isTwoPlayerMode)
        {
            HandleTwoPlayerControls();
        }
        else
        {
            HandleSinglePlayerControls();
        }
    }

    private void HandleSinglePlayerControls()
    {
        // Allow either W/S or Up/Down keys to control the paddle
        float input = 0f;

        if (isPlayer1)
        {
            input = Input.GetAxisRaw("Vertical"); // W/S or Up/Down for Player 1 in Single-Player
        }

        MovePaddle(input);
    }

    private void HandleTwoPlayerControls()
    {
        float input = 0f;

        if (isPlayer1)
        {
            // Player 1 uses W/S keys in 2-Player Mode
            input = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else
        {
            // Player 2 uses Up/Down Arrow keys in 2-Player Mode
            input = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }

        MovePaddle(input);
    }

    private void MovePaddle(float input)
    {
        // Calculate movement
        float move = input * speed * Time.deltaTime;

        // Update paddle position
        Vector3 paddlePosition = transform.position;
        paddlePosition.y += move;

        // Clamp position to stay within boundaries
        paddlePosition.y = Mathf.Clamp(paddlePosition.y, -boundaryY, boundaryY);

        // Apply the updated position
        transform.position = paddlePosition;
    }
}
