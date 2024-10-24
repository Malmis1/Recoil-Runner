using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScript : MonoBehaviour {
    [Tooltip("The win menu object")]
    public GameObject winMenu;

    public TextMeshProUGUI finalTimeText;

    public void PauseAndShowWinMenu() {
        Time.timeScale = 0;
        winMenu.SetActive(true);

        SetFinalTimeText();
    }

    public void SetFinalTimeText() {
        float timeSinceLoad = Time.timeSinceLevelLoad;

        int seconds = Mathf.FloorToInt(timeSinceLoad);

        int milliseconds = Mathf.FloorToInt((timeSinceLoad - seconds) * 100);

        string timeString = string.Format("{0:00}:{1:00}", seconds, milliseconds);

        if (finalTimeText != null) {
            finalTimeText.text = timeString;
        } else {
            Debug.LogWarning("FinalTimeText is not assigned in the WinScript.");
        }
    }
}
