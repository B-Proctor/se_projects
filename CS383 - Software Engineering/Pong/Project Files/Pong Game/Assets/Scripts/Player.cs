using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float boundaryY = 4.5f;

    public bool isPlayer1;
    private bool isTwoPlayerMode;

    void Update()
    {
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
        float input = 0f;

        if (isPlayer1)
        {
            input = Input.GetAxisRaw("Vertical");
        }

        MovePaddle(input);
    }

    private void HandleTwoPlayerControls()
    {
        float input = 0f;

        if (isPlayer1)
        {
            input = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        }
        else
        {
            input = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;
        }

        MovePaddle(input);
    }

    private void MovePaddle(float input)
    {
        float move = input * speed * Time.deltaTime;

        Vector3 paddlePosition = transform.position;
        paddlePosition.y += move;

        paddlePosition.y = Mathf.Clamp(paddlePosition.y, -boundaryY, boundaryY);

        transform.position = paddlePosition;
    }
}
