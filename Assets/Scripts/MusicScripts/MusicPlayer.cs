using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType<MusicPlayer>().Length > 1) {
            Destroy(gameObject);
        }
    }
}
