using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Tooltip("The movement controller for the enemy")]
    public EnemyController controller;

    float horizontalMove = 0f;
    bool jump = false;

    void Update() {
        horizontalMove = 0.2f;

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
}
