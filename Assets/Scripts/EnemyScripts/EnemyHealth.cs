using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    [Tooltip("The controller for the enemy")]
    public EnemyController controller;

    public int health = 1;

    void Update() {
        if (health <= 0) {
            controller.EnemyDie();
        }
    }

    public void TakeDamage(int amount) {
        health += amount;
    }

    public void AddHealth(int amount) {
        health += amount;
    }
}
