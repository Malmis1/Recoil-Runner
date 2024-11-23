using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoToSceneLoader : MonoBehaviour
{
    public string nextSceneName; 
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd; 
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); 
    }
}
