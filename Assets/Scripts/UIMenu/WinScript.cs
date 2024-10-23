using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour {
    [Tooltip("The win menu object")]
    public GameObject winMenu;

    public void PauseAndShowWinMenu() {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }
}
