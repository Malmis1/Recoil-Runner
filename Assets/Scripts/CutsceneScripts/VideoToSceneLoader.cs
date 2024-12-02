using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoToSceneLoader : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        string nextSceneName = PlayerPrefs.GetString("NextSceneAfterIntro", "Tutorial"); // Default to Tutorial
        SceneManager.LoadScene(nextSceneName);
    }
}
