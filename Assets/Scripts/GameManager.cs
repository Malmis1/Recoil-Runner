using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>();
                if (instance == null) {
                    GameObject go = new GameObject("GameManager");
                    instance = go.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    public TextMeshProUGUI killCountText;
    private int killCount = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            if (killCountText != null) {
                instance.killCountText = killCountText;
            }
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