using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Tooltip("The movement controller for the enemy")]
    public EnemyController controller;

    float horizontalMove = 0.2f;
    bool jump = false;

    void Update() {

        if (!controller.IsMoving()) {
            SwitchDirection();
        }

        RandomJump();
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    void SwitchDirection() {
        horizontalMove = horizontalMove * (-1.0f);
    }

    void RandomJump() {
        int random = Random.Range(0, 50);
        
        if (random == 0) {
            jump = true;
        }
    }
}
