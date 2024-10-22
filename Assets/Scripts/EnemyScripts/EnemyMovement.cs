using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Tooltip("The movement controller for the enemy")]
    public EnemyController controller;

    float horizontalMove = 0f;
    bool jump = false;

    void Update() {
        horizontalMove = 0.2f;
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
