using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public void LoadLevel(string levelToLoad) {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
