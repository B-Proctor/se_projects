using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Gameplay Settings")]
    public Toggle twoPlayerModeToggle;
    public Toggle drbcModeToggle;
    public TMP_Dropdown difficultyDropdown;

    [Header("Gameplay Objects")]
    public GameObject enemyAI;
    public GameObject player2;
    public GameObject secretWall;

    private bool isTwoPlayerMode = false;
    private bool isDRBCMode = false;

    void Start()
    {
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
            difficultyDropdown.interactable = false;
            drbcModeToggle.interactable = false;
        }
        else
        {
            enemyAI.SetActive(true);
            player2.SetActive(false);
            difficultyDropdown.interactable = true;
            drbcModeToggle.interactable = true;
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
            difficultyDropdown.interactable = false;
            twoPlayerModeToggle.interactable = false;
            Debug.Log("DRBC Mode enabled: Impossible to lose.");
            enemyAI.SetActive(false);
            secretWall.SetActive(true);
        }
        else
        {
            Debug.Log("DRBC Mode disabled: Normal gameplay.");
            twoPlayerModeToggle.interactable = true;
            difficultyDropdown.interactable = true;
            enemyAI.SetActive(true);
            secretWall.SetActive(false);

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
