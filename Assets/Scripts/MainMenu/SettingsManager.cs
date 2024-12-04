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
            // Initialize sliders with current AudioManager values
            masterVolumeSlider.value = audioManager.masterVolume;
            musicSlider.value = audioManager.musicVolume;

            // Update values dynamically
            masterVolumeSlider.onValueChanged.AddListener(audioManager.SetMasterVolume);
            musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("AudioManager not found in the scene.");
        }
    }
}
