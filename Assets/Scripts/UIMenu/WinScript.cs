using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    [Tooltip("The win menu object")]
    public GameObject winMenu;
    public TextMeshProUGUI finalTimeText;

    [Tooltip("Text for 'Star Time'.")]
    public TextMeshProUGUI starTimeText;

    [Tooltip("Text for 'Best Time'.")]
    public TextMeshProUGUI bestTimeText;

    [Tooltip("Text for 'Kills'.")]
    public TextMeshProUGUI killsText;

    [Tooltip("Star objects representing 1, 2, and 3 stars.")]
    public GameObject[] stars;

    public void PauseAndShowWinMenu()
    {
        Time.timeScale = 0;
        winMenu.SetActive(true);

        SetFinalTimeText();
        UpdateAdditionalText();
        CalculateAndShowStars();
    }

    public void SetFinalTimeText()
    {
        float timeSinceLoad = Time.timeSinceLevelLoad;

        int seconds = Mathf.FloorToInt(timeSinceLoad);
        int milliseconds = Mathf.FloorToInt((timeSinceLoad - seconds) * 100);

        // Format the current time
        string currentTimeString = string.Format("{0:00}:{1:00}", seconds, milliseconds);

        finalTimeText.text = "Time: " + currentTimeString;

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

    public void UpdateAdditionalText()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        KillCounter killCounter = KillCounter.Instance;

        if (levelManager == null || killCounter == null)
        {
            Debug.LogError("LevelManager or KillCounter is not found in the scene.");
            return;
        }

        // Update 'Star Time'
        // Format 'Time to beat' as SS:MS
        float maxTimeForStars = levelManager.maxTimeForStars;
        int starSeconds = Mathf.FloorToInt(maxTimeForStars);
        int starMilliseconds = Mathf.FloorToInt((maxTimeForStars - starSeconds) * 100);
        string starTimeFormatted = string.Format("{0:00}:{1:00}", starSeconds, starMilliseconds);

        starTimeText.text = "Star Time: " + starTimeFormatted;

        // Update 'Best Time'
        string pbTime = PlayerPrefs.GetString("LevelTime_" + levelManager.currentLevel, "N/A");
        bestTimeText.text = "Best Time: " + pbTime;

        // Update 'Kills'
        killsText.text = "Kills: " + killCounter.KillCount + "/" + killCounter.TotalEnemies;
    }


    public void CalculateAndShowStars()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        KillCounter killCounter = KillCounter.Instance;

        if (levelManager == null || killCounter == null)
        {
            Debug.LogError("LevelManager or KillCounter is not found in the scene.");
            return;
        }

        int currentLevel = levelManager.currentLevel;
        float maxTimeForStar = levelManager.maxTimeForStars;

        int starsEarned = 0;

        // Star 1: Level completion
        starsEarned++;

        // Star 2: All kills achieved
        if (killCounter.KillCount >= killCounter.TotalEnemies)
        {
            starsEarned++;
        }

        // Star 3: Finish under max time
        float timeSinceLoad = Time.timeSinceLevelLoad;
        if (timeSinceLoad <= maxTimeForStar)
        {
            starsEarned++;
        }

        int bestStars = PlayerPrefs.GetInt("LevelStars_" + currentLevel, 0);
        if (starsEarned > bestStars)
        {
            PlayerPrefs.SetInt("LevelStars_" + currentLevel, starsEarned);
            PlayerPrefs.Save();
        }

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starsEarned)
            {
                stars[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                stars[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
