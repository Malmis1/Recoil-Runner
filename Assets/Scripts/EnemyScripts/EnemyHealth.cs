using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Tooltip("The controller for the enemy")]
    public EnemyController controller;

    public float health = 1.0f;

    void Update() {
        if (health <= 0f) {
            controller.EnemyDie();
        }
    }

    public void TakeDamage(float amount) {
        health -= amount;
        Debug.Log("Enemy health: " + health);
    }

    public void AddHealth(float amount) {
        health += amount;
    }
}
