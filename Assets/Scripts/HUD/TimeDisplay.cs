using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [Tooltip("Reference to the text element which is displaying the time.")]
    public TextMeshProUGUI timeText;

    void Update() {
        float timeSinceLoad = Time.timeSinceLevelLoad;

        int seconds = Mathf.FloorToInt(timeSinceLoad);

        int milliseconds = Mathf.FloorToInt((timeSinceLoad - seconds) * 100);

        string timeString = string.Format("Text: {0:00}:{1:00}", seconds, milliseconds);

        timeText.text = timeString;
    }
}
