using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public EnemyController controller;
    public float health = 1.0f;
    private bool isDead = false;
    private bool initialized = false;
    private float initDelay = 0.1f;

    void Start() {
        // Give time for physics to initialize
        Invoke("SetInitialized", initDelay);
    }

    void SetInitialized() {
        initialized = true;
    }

    void Update() {
        if (!initialized) return;
        
        if (health <= 0f && !isDead) {
            isDead = true;
            GameManager.Instance.IncrementKillCount();
            controller.EnemyDie();
        }
    }

    public void TakeDamage(float amount) {
        if (!isDead && initialized) {
            health -= amount;
        }
    }

    public void AddHealth(float amount) {
        health += amount;
    }
}