using UnityEngine;

public class KillCounter : MonoBehaviour {
    public static KillCounter Instance;

    public int KillCount { get; private set; } = 0;

    [Tooltip("The total amount of enemies in the level")]
    public int TotalEnemies = 0;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void IncreaseKillAmount() {
        KillCount = KillCount + 1;
    }
}