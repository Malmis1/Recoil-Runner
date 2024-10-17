using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {
    [Tooltip("The pause menu object")]
    public GameObject pauseMenu;

    private bool isPaused;

    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused) {
                PauseGame();
            } else {
                UnPauseGame();
            }
        }
        
    }

    private void PauseGame() {
        isPaused = true;

        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnPauseGame() {
        isPaused = false;

        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
