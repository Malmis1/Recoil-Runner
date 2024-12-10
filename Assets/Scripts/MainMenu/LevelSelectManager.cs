using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonManager : MonoBehaviour
{
    public Button[] levelButtons;
    public Color lockedColor = new Color(144f / 255f, 144f / 255f, 144f / 255f);
    public Color unlockedColor = Color.white;

    void Start()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool isLocked = (i + 1) > levelsUnlocked;
            SetButtonState(levelButtons[i], i + 1, isLocked);
        }
    }

    void SetButtonState(Button button, int level, bool isLocked)
    {
        Transform textTransform = button.transform.Find("Text");
        Transform timerTransform = button.transform.Find("Time");
        Transform lockTransform = button.transform.Find("Lock");

        // Handle Text visibility
        if (textTransform != null)
            textTransform.gameObject.SetActive(!isLocked);

        // Handle Timer visibility and content
        if (timerTransform != null)
        {
            TextMeshProUGUI timeText = timerTransform.GetComponent<TextMeshProUGUI>();
            if (timeText != null)
            {
                if (!isLocked)
                {
                    string savedTime = PlayerPrefs.GetString("LevelTime_" + level, "");
                    timeText.text = savedTime;
                }
                else
                {
                    timeText.text = ""; // Clear the time if locked
                }
                timerTransform.gameObject.SetActive(!isLocked);
            }
        }

        // Handle Lock visibility
        if (lockTransform != null)
            lockTransform.gameObject.SetActive(isLocked);

        // Set button colors and interactivity
        ColorBlock cb = button.colors;
        cb.normalColor = isLocked ? lockedColor : unlockedColor;
        cb.highlightedColor = isLocked ? lockedColor : unlockedColor;
        cb.pressedColor = isLocked ? lockedColor : unlockedColor;
        cb.disabledColor = isLocked ? lockedColor : unlockedColor;
        button.colors = cb;

        button.interactable = !isLocked;

        UpdateStars(button, level, isLocked);
    }

    void UpdateStars(Button button, int level, bool isLocked)
    {
        for (int starIndex = 1; starIndex <= 3; starIndex++)
        {
            Transform starTransform = button.transform.Find($"Star {starIndex}");
            if (starTransform != null)
            {
                Image starImage = starTransform.GetComponent<Image>();
                if (starImage != null)
                {
                    if (isLocked)
                    {
                        // Hide stars if the level is locked
                        starImage.color = Color.clear;
                    }
                    else
                    {
                        // Show stars based on earned stars
                        int starsEarned = PlayerPrefs.GetInt($"LevelStars_{level}", 0);
                        starImage.color = (starIndex <= starsEarned) ? Color.white : Color.gray;
                    }
                }
            }
        }
    }
}
