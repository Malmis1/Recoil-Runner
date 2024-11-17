using UnityEngine;
using UnityEngine.UI;

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
            SetButtonState(levelButtons[i], isLocked);
        }
    }

    void SetButtonState(Button button, bool isLocked)
    {
        Transform textTransform = button.transform.Find("Text");
        Transform timerTransform = button.transform.Find("Timer");
        Transform lockTransform = button.transform.Find("Lock");

        if (textTransform != null)
            textTransform.gameObject.SetActive(!isLocked);

        if (timerTransform != null)
            timerTransform.gameObject.SetActive(!isLocked);

        if (lockTransform != null)
            lockTransform.gameObject.SetActive(isLocked);

        ColorBlock cb = button.colors;
        cb.normalColor = isLocked ? lockedColor : unlockedColor;
        cb.highlightedColor = isLocked ? lockedColor : unlockedColor;
        cb.pressedColor = isLocked ? lockedColor : unlockedColor;
        cb.disabledColor = isLocked ? lockedColor : unlockedColor;
        button.colors = cb;

        button.interactable = !isLocked;
    }
}
