using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Tooltip("The movement controller for the enemy")]
    public EnemyController controller;

    float horizontalMove = 0.2f;
    bool jump = false;
    private float jumpTimer = 0.0f;

    void Update() {
        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0f) {
            RandomJump();
        }

        if (!controller.IsMoving()) {
            SwitchDirection();
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    void SwitchDirection() {
        horizontalMove = horizontalMove * (-1.0f);
    }

    void RandomJump() {
        jumpTimer = Random.Range(2f, 5f);
        jump = true;
    }
}
