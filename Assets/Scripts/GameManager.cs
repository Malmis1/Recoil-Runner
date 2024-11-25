using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI killCountText;
    private int killCount = 0;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void IncrementKillCount() {
        killCount++;
        UpdateKillCountDisplay();
    }

    void UpdateKillCountDisplay() {
        if (killCountText != null) {
            killCountText.text = $"Kills: {killCount}";
        }
    }
}