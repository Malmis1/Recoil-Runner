using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMenu : MonoBehaviour
{
    [Tooltip("The main UI page to hide when the TutorialQuestionPage is opened. Only needed if OnPlayButtonPressed() is called. ")]
    public GameObject MainPage;

    [Tooltip("The UI page shown when the play button is pressed. Only needed if OnPlayButtonPressed() is called.")]
    public GameObject TutorialQuestionPage;

    [Tooltip("The button to load the intro. This will be enabled or disabled based on the IntroCompleted state. Only needed if theres is a loadintro button.")]
    public GameObject LoadIntroButton;
    [Tooltip("The quit button to deactivate in WebGL builds. Only needed if there is a quit button.")]
    public GameObject QuitButton;

    private void Start()
    {
        UpdateLoadIntroButtonState();
        DisableQuitButtonForWebGL();
    }

    public void LoadLevel(string levelToLoad)
    {
        if (!PlayerPrefs.HasKey("IntroCompleted"))
        {
            PlayerPrefs.SetString("NextSceneAfterIntro", levelToLoad);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Intro");
        }
        else
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public void LoadNextLevel()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);

        if (levelsUnlocked == 21)
        {
            SceneManager.LoadScene("CityLevel1");
            return;
        }

        string nextLevelScene = GetLevelName(levelsUnlocked);

        if (Application.CanStreamedLevelBeLoaded(nextLevelScene))
        {
            SceneManager.LoadScene(nextLevelScene);
        }
        else
        {
            Debug.LogError($"Scene '{nextLevelScene}' does not exist in Build Settings.");
        }
    }

    public void SkipIntro()
    {
        if (PlayerPrefs.HasKey("IntroCompleted") && PlayerPrefs.GetInt("IntroCompleted") == 1)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        PlayerPrefs.SetInt("IntroCompleted", 1);
        PlayerPrefs.Save();

        string nextScene = PlayerPrefs.GetString("NextSceneAfterIntro", "Tutorial");
        SceneManager.LoadScene(nextScene);
    }

    public void QuitGame()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
                            Application.Quit();

#endif
    }

    public void OnPlayButtonPressed()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        if (levelsUnlocked > 0)
        {
            LoadNextLevel();
        }
        else
        {
            if (TutorialQuestionPage != null && MainPage != null)
            {
                TutorialQuestionPage.SetActive(true);
                MainPage.SetActive(false);
            }
            else
            {
                Debug.LogWarning("TutorialQuestionPage or MainPage is not assigned in the inspector.");
            }
        }
    }

    private string GetLevelName(int levelNumber)
    {
        if (levelNumber <= 10)
        {
            return $"CityLevel{levelNumber}";
        }
        else
        {
            int forestLevel = levelNumber - 10;
            return $"ForestLevel{forestLevel}";
        }
    }

    private void UpdateLoadIntroButtonState()
    {
        if (LoadIntroButton != null)
        {
            bool isIntroCompleted = PlayerPrefs.HasKey("IntroCompleted") && PlayerPrefs.GetInt("IntroCompleted") == 1;
            LoadIntroButton.SetActive(isIntroCompleted);
        }
    }

    private void DisableQuitButtonForWebGL()
    {
#if (UNITY_WEBGL)
        if (QuitButton != null)
        {
            QuitButton.SetActive(false);
        }
#endif
    }

    public void OnLoadIntroButtonPressed()
    {
        SceneManager.LoadScene("Intro");
    }
}
