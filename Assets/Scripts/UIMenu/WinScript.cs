using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScript : MonoBehaviour
{
    [Tooltip("The win menu object")]
    public GameObject winMenu;

    public TextMeshProUGUI finalTimeText;

    public void PauseAndShowWinMenu()
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);

        SetFinalTimeText();
    }

    public void SetFinalTimeText()
    {
        float timeSinceLoad = Time.timeSinceLevelLoad;

        int seconds = Mathf.FloorToInt(timeSinceLoad);
        int milliseconds = Mathf.FloorToInt((timeSinceLoad - seconds) * 100);

        // Format the current time
        string currentTimeString = string.Format("{0:00}:{1:00}", seconds, milliseconds);

        finalTimeText.text = "Time:" + currentTimeString;

        // Get the current level from LevelManager
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            int currentLevel = levelManager.currentLevel;

            // Get the saved best time for the level
            string savedBestTime = PlayerPrefs.GetString("LevelTime_" + currentLevel, "");

            // Compare the current time with the saved best time
            if (string.IsNullOrEmpty(savedBestTime) || IsCurrentTimeBetter(currentTimeString, savedBestTime))
            {
                // Save the new best time
                PlayerPrefs.SetString("LevelTime_" + currentLevel, currentTimeString);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene.");
        }
    }

    private bool IsCurrentTimeBetter(string currentTime, string savedTime)
    {
        // Parse the saved time (XX:YY)
        string[] savedParts = savedTime.Split(':');
        int savedSeconds = int.Parse(savedParts[0]);
        int savedMilliseconds = int.Parse(savedParts[1]);

        // Parse the current time (XX:YY)
        string[] currentParts = currentTime.Split(':');
        int currentSeconds = int.Parse(currentParts[0]);
        int currentMilliseconds = int.Parse(currentParts[1]);

        // Compare times
        if (currentSeconds < savedSeconds) return true;
        if (currentSeconds == savedSeconds && currentMilliseconds < savedMilliseconds) return true;

        return false;
    }

}
