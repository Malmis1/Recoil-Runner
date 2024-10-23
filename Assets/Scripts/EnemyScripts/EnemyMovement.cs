using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [Tooltip("The movement controller for the enemy")]
    public EnemyController controller;

    private float horizontalMove = 0.2f;
    private bool jump = false;
    private float jumpTimer = 0.0f;
    private float walkTimer = 0.0f;
    private bool isWalking = true;

    void Update() {
        controller.DetectPlayer();

        if (controller.getPlayerTransform() != null) { // Player is detected 
            MoveTowardsPlayer();
        } else { // Player is not detected (wander)
            if (horizontalMove > 0f) {
                jumpTimer -= Time.deltaTime;
            }
            walkTimer -= Time.deltaTime;

            if (jumpTimer <= 0f) {
                RandomJump();
            }

            if (walkTimer <= 0f) {
                RandomWalk();
            }

            if (!controller.IsMoving() && isWalking) {
                SwitchDirection();
            }
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    void MoveTowardsPlayer() {
        Transform newPlayerTransform = controller.getPlayerTransform();
        float direction = newPlayerTransform.position.x - transform.position.x;
        horizontalMove = Mathf.Sign(direction) * 0.2f; // Right positive. Left negative.
    }

    void SwitchDirection() {
        horizontalMove = horizontalMove * (-1.0f);
    }

    void RandomJump() {
        jumpTimer = Random.Range(2f, 6f);
        jump = true;
    }

    void RandomWalk() {
        walkTimer = Random.Range(4f, 8f);

        if (horizontalMove > 0f) {
            horizontalMove = 0.0f;
            isWalking = false;
        } else {
            horizontalMove = 0.2f;
            isWalking = true;
        }
    }
}
