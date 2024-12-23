using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [Tooltip("The AudioMixer controlling game audio")]
    public AudioMixer audioMixer;
    [Tooltip("The AudioMixerGroup for all audio sources.")]
    public AudioMixerGroup masterGroup;
    [Tooltip("The AudioMixerGroup for all music audio sources.")]
    public AudioMixerGroup musicGroup;
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;

    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    private static bool isInitialized = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (!isInitialized)
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);

            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);

            isInitialized = true;
        }
        else
        {
            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
        }

        AssignAllAudioSourcesToMasterGroup();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Assign AudioSources for the newly loaded scene
        AssignAllAudioSourcesToMasterGroup();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;

        float dbValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("MasterVolume", dbValue);

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        float dbValue = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20;
        audioMixer.SetFloat("MusicVolume", dbValue);

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    private void AssignAllAudioSourcesToMasterGroup()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.CompareTag("Music"))
            {
                audioSource.outputAudioMixerGroup = musicGroup;
            }
            else
            {
                audioSource.outputAudioMixerGroup = masterGroup;
            }
        }
    }
}
