using UnityEngine;
using TMPro;

public class LevelNameText : MonoBehaviour
{
    [Tooltip("The TextMeshProUGUI component to display the level name.")]
    public TextMeshProUGUI levelNameText;

    private void Start()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            string levelName = GetLevelName(levelManager.currentLevel);
            levelNameText.text = levelName;
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene.");
        }
    }

    private string GetLevelName(int levelNumber)
    {
        if (levelNumber <= 10)
        {
            return $"City Level {levelNumber}";
        }
        else
        {
            int forestLevel = levelNumber - 10;
            return $"Forest Level {forestLevel}";
        }
    }
}
