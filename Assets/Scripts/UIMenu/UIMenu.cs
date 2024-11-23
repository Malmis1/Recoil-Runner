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
            PlayerPrefs.Save();

            SceneManager.LoadScene("Intro");
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
