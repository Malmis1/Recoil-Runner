using UnityEngine;

public class EnemyAnimator : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The enemy controller script to read state information from")]
    public EnemyController enemyController;
    
    [Tooltip("The animator component that controls the enemy's animations")]
    public Animator animator;

    void Start() {
        UpdateAnimationState();
    }

    void Update() {
        UpdateAnimationState();
    }

    private void UpdateAnimationState() {
        if (animator == null || enemyController == null) {
            return;
        }

        ResetAnimations();

        switch (enemyController.state) {
            case EnemyController.EnemyState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case EnemyController.EnemyState.Walk:
                animator.SetBool("isWalking", true);
                break;
            case EnemyController.EnemyState.Jump:
                animator.SetBool("isJumping", true);
                break;
            case EnemyController.EnemyState.Fall:
                animator.SetBool("isFalling", true);
                break;
            case EnemyController.EnemyState.Dead:
                animator.SetTrigger("isDead");
                break;
        }
    }

    private void ResetAnimations() {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
    }
}
