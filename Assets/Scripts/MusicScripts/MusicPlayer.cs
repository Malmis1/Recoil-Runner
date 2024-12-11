using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource mainMenuAudio;
    private AudioSource gameplayAudio;
    private bool isInMainMenu = true;

    [SerializeField] private AudioClip gameplayMusicClip;
    [SerializeField] private AudioClip cutsceneMusicClip; // Reference to cutscene music

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        // Get the existing AudioSource for menu music
        mainMenuAudio = GetComponent<AudioSource>();
        
        // Create and setup gameplay audio source
        gameplayAudio = gameObject.AddComponent<AudioSource>();
        gameplayAudio.loop = true;
        gameplayAudio.playOnAwake = false;
        gameplayAudio.clip = gameplayMusicClip;
        
        // Check for duplicate music players
        if (FindObjectsOfType<MusicPlayer>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (gameplayMusicClip == null)
        {
            Debug.LogWarning("Gameplay music clip not assigned in MusicPlayer!");
        }
        if (cutsceneMusicClip == null)
        {
            Debug.LogWarning("Cutscene music clip not assigned in MusicPlayer!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're entering the Intro scene
        if (scene.name == "Intro")
        {
            StopAllMusic();
            // Let the AudioSource in the Intro scene handle the cutscene music
            return;
        }

        bool newSceneIsMainMenu = scene.name.ToLower().Contains("mainmenu");

        // Only switch music if we're entering or leaving main menu
        if (newSceneIsMainMenu != isInMainMenu)
        {
            if (newSceneIsMainMenu)
            {
                // Switching to main menu
                gameplayAudio.Stop();
                mainMenuAudio.Play();
            }
            else
            {
                // Leaving main menu
                mainMenuAudio.Stop();
                if (gameplayMusicClip != null)  
                {
                    gameplayAudio.Play();
                }
            }
            isInMainMenu = newSceneIsMainMenu;
        }
    }

    private void StopAllMusic()
    {
        mainMenuAudio.Stop();
        gameplayAudio.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}