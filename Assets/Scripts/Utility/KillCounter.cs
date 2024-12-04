using UnityEngine;

public class KillCounter : MonoBehaviour {
    public static KillCounter Instance;

    public int KillCount { get; private set; } = 0;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseKillAmount() {
        KillCount = KillCount + 1;
        Debug.Log("Kill Count is: " + KillCount);
    }
}