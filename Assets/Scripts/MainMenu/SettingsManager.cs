using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        if (audioManager != null)
        {
            // Set slider values from saved preferences
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

            // Initialize audio settings
            audioManager.SetMasterVolume(masterVolumeSlider.value);
            audioManager.SetMusicVolume(musicSlider.value);

            // Add listeners to sliders
            masterVolumeSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
            musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene.");
        }
    }
}
