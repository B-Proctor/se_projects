using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the pause menu GameObject
    public GameObject optionsMenuUI; // Reference to the options menu GameObject
    public GameObject controlsMenuUI; // Reference to the controls menu GameObject
    public GameObject copyrightMenuUI; // Reference to the copyright disclaimer menu GameObject

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when Esc or Space is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        CloseAllPanels(); // Close all menu panels
        Time.timeScale = 1f; // Resume the game
        isPaused = false;
    }

    public void Pause()
    {
        CloseAllPanels(); // Ensure all panels are closed
        pauseMenuUI.SetActive(true); // Open the pause menu
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void OpenOptions()
    {
        CloseAllPanels();
        optionsMenuUI.SetActive(true); // Open the options menu
    }

    public void OpenControls()
    {
        CloseAllPanels();
        controlsMenuUI.SetActive(true); // Open the controls menu
    }

    public void OpenCopyrightDisclaimer()
    {
        CloseAllPanels();
        copyrightMenuUI.SetActive(true); // Open the copyright disclaimer menu
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game..."); // Log the exit action
        Application.Quit(); // Exit the application
    }

    private void CloseAllPanels()
    {
        pauseMenuUI?.SetActive(false);
        optionsMenuUI?.SetActive(false);
        controlsMenuUI?.SetActive(false);
        copyrightMenuUI?.SetActive(false);
    }
}
