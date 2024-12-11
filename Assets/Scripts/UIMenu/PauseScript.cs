using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour {
    [Tooltip("The pause menu object")]
    public GameObject pauseMenu;

    private bool isPaused;

    void Start() {
        UnPauseGame();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Time.timeScale != 0) { // This is for making sure that it is not possible to pause if the game is won or game over.
                if (!isPaused) {
                    PauseGame();
                } else {
                    UnPauseGame();
                }
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
