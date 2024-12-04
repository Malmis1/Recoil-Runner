using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounterUI : MonoBehaviour {
    [Tooltip("The kill counter text (using TMPro)")]
    [SerializeField] private TextMeshProUGUI killCountText;

    private void Update() {
        if (KillCounter.Instance != null) {
            killCountText.text = "Kills: " + KillCounter.Instance.KillCount;
        }
    }
}