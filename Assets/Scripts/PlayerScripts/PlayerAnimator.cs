using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The player controller script to read state information from")]
    public PlayerController playerController;

    [Tooltip("The animator component that controls the player's animations")]
    public Animator animator;

    [Tooltip("The animator controller for when the player is holding a gun")]
    public RuntimeAnimatorController playerControllerNoHands;

    [Tooltip("The animator controller for when the player is not holding a gun")]
    public RuntimeAnimatorController playerControllerHands;

    [Tooltip("The gun GameObject")]
    public GameObject gunObject;

    private Rigidbody2D rb;

    private GunScript gunScript;

    void Start()
    {
        rb = playerController.GetComponent<Rigidbody2D>();
        gunScript = gunObject.GetComponent<GunScript>();
        ReadPlayerStateAndAnimate();
    }


    void Update()
    {
        if (gunObject != null)
        {
            if (gunObject.activeSelf)
            {
                animator.runtimeAnimatorController = playerControllerNoHands;
            }
            else
            {
                animator.runtimeAnimatorController = playerControllerHands;
            }
        }

        ReadPlayerStateAndAnimate();
    }

    void ReadPlayerStateAndAnimate()
    {
        if (animator == null || gunScript == null)
        {
            return;
        }

        ResetAnimations();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootingDirection = (mousePosition - transform.position).normalized;

        bool isMovingRight = rb.velocity.x > 0;
        bool isMovingLeft = rb.velocity.x < 0;

        float angle = Vector2.SignedAngle(Vector2.right, shootingDirection);

        if (gunScript.IsFiring())
        {
            animator.SetBool("isShooting", true);
            if (angle >= 45 && angle < 135)
            {
                UpdateMovementAnimation();
            }
            else if (angle < -45 && angle > -135)
            {
                animator.SetBool("isShootingDown", true);
            }
            else if (angle >= -45 && angle < 45)
            {
                animator.SetBool("isShootingHorizontal", true);
            }
            else if (angle < -135 || angle >= 135)
            {
                animator.SetBool("isShootingHorizontal", true);
            }
        }
        else
        {
            animator.SetBool("isShooting", false);
            UpdateMovementAnimation();
        }

        animator.SetBool("isDead", playerController.state == PlayerController.PlayerState.Dead);
    }

    private void ResetAnimations()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShootingHorizontal", false);
        animator.SetBool("isShootingDown", false);
    }

    private void UpdateMovementAnimation()
    {
        switch (playerController.state)
        {
            case PlayerController.PlayerState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case PlayerController.PlayerState.Jump:
                animator.SetBool("isJumping", true);
                break;
            case PlayerController.PlayerState.Fall:
                animator.SetBool("isFalling", true);
                break;
            case PlayerController.PlayerState.Walk:
                animator.SetBool("isWalking", true);
                break;
        }
    }
}
