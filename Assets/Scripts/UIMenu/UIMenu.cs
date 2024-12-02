using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public void LoadLevel(string levelToLoad)
    {
        if (!PlayerPrefs.HasKey("IntroCompleted"))
        {
            PlayerPrefs.SetInt("IntroCompleted", 1);
            PlayerPrefs.SetString("NextSceneAfterIntro", levelToLoad);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Intro");
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void SkipIntro()
    {
        string nextScene = PlayerPrefs.GetString("NextSceneAfterIntro", "Tutorial");
        SceneManager.LoadScene(nextScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
