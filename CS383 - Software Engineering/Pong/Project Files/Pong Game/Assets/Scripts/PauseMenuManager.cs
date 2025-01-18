using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    /*references*/
    public GameObject pauseMenuUI; 
    public GameObject optionsMenuUI; 
    public GameObject controlsMenuUI; 
    public GameObject copyrightMenuUI;

    private bool isPaused = false;

    void Update()
    {
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
        CloseAllPanels(); 
        Time.timeScale = 1f; 
        isPaused = false;
    }

    public void Pause()
    {
        CloseAllPanels(); 
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void OpenOptions()
    {
        CloseAllPanels();
        optionsMenuUI.SetActive(true); 
    }

    public void OpenControls()
    {
        CloseAllPanels();
        controlsMenuUI.SetActive(true); 
    }

    public void OpenCopyrightDisclaimer()
    {
        CloseAllPanels();
        copyrightMenuUI.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game..."); 
        Application.Quit(); 
    }

    private void CloseAllPanels()
    {
        pauseMenuUI?.SetActive(false);
        optionsMenuUI?.SetActive(false);
        controlsMenuUI?.SetActive(false);
        copyrightMenuUI?.SetActive(false);
    }
}
