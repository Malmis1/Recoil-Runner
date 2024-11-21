using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource mainMenuAudio;
    private AudioSource gameplayAudio;
    private bool isInMainMenu = true;

    [SerializeField] private AudioClip gameplayMusicClip;  

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        // Get the existing AudioSource for menu music
        mainMenuAudio = GetComponent<AudioSource>();
        
        // Create and setup gameplay audio source
        gameplayAudio = gameObject.AddComponent<AudioSource>();
        gameplayAudio.loop = true;
        gameplayAudio.playOnAwake = false;
        gameplayAudio.clip = gameplayMusicClip;  // Assign the clip
        
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
        // Validate that we have our gameplay music
        if (gameplayMusicClip == null)
        {
            Debug.LogWarning("Gameplay music clip not assigned in MusicPlayer!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}