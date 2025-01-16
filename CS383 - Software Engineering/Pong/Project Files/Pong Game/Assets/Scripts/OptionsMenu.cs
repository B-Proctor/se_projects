using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider sfxVolumeSlider; // Reference to the SFX volume slider
    public Slider musicVolumeSlider; // Reference to the music volume slider
    public AudioSource sfxSource; // Reference to the AudioSource for SFX
    public AudioSource musicSource; // Reference to the AudioSource for music

    [Header("Gameplay Settings")]
    public Toggle twoPlayerModeToggle; // Reference to the 2-Player Mode checkbox
    public Toggle drbcModeToggle; // Reference to the DRBC Mode checkbox
    public TMP_Dropdown difficultyDropdown; // Reference to the TMP difficulty dropdown

    [Header("Gameplay Objects")]
    public GameObject enemyAI; // Reference to the enemy paddle
    public GameObject player2; // Reference to the second player's paddle

    private bool isTwoPlayerMode = false;
    private bool isDRBCMode = false;

    void Start()
    {
        // Load saved settings and apply them
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        twoPlayerModeToggle.isOn = PlayerPrefs.GetInt("TwoPlayerMode", 0) == 1;
        drbcModeToggle.isOn = PlayerPrefs.GetInt("DRBCMode", 0) == 1;
        difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty", 1);

        ApplyAudioSettings();
        ApplyTwoPlayerMode();
        ApplyDRBCMode();
        ApplyDifficulty();
    }

    public void OnSFXVolumeChanged()
    {
        float volume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        sfxSource.volume = volume;
    }

    public void OnMusicVolumeChanged()
    {
        float volume = musicVolumeSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        musicSource.volume = volume;
    }

    public void OnTwoPlayerModeToggled()
    {
        isTwoPlayerMode = twoPlayerModeToggle.isOn;
        PlayerPrefs.SetInt("TwoPlayerMode", isTwoPlayerMode ? 1 : 0);
        ApplyTwoPlayerMode();
    }

    private void ApplyTwoPlayerMode()
    {
        if (isTwoPlayerMode)
        {
            enemyAI.SetActive(false);
            player2.SetActive(true);
            difficultyDropdown.interactable = false; // Disable the difficulty dropdown
        }
        else
        {
            enemyAI.SetActive(true);
            player2.SetActive(false);
            difficultyDropdown.interactable = true; // Enable the difficulty dropdown
        }
    }

    public void OnDRBCModeToggled()
    {
        isDRBCMode = drbcModeToggle.isOn;
        PlayerPrefs.SetInt("DRBCMode", isDRBCMode ? 1 : 0);
        ApplyDRBCMode();
    }

    private void ApplyDRBCMode()
    {
        if (isDRBCMode)
        {
            Debug.Log("DRBC Mode enabled: Impossible to lose.");
        }
        else
        {
            Debug.Log("DRBC Mode disabled: Normal gameplay.");
        }
    }

    public void OnDifficultyChanged()
    {
        int selectedDifficulty = difficultyDropdown.value;
        PlayerPrefs.SetInt("Difficulty", selectedDifficulty);
        ApplyDifficulty();
    }

    private void ApplyDifficulty()
    {
        if (enemyAI.TryGetComponent(out Enemy enemyScript))
        {
            enemyScript.SetDifficulty(difficultyDropdown.value);
        }
    }

    private void ApplyAudioSettings()
    {
        sfxSource.volume = sfxVolumeSlider.value;
        musicSource.volume = musicVolumeSlider.value;
    }
}
