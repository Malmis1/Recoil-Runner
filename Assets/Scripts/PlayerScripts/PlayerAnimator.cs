using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles interactions with the animator component of the player
/// It reads the player's state from the controller and animates accordingly
/// </summary>
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

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first update
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        rb = playerController.GetComponent<Rigidbody2D>();
        ReadPlayerStateAndAnimate();
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        // Check if the gun is active or not
        if (gunObject != null)
        {
            if (gunObject.activeSelf)
            {
                // Gun is active, switch to the no-hands animator controller
                animator.runtimeAnimatorController = playerControllerNoHands;
            }
            else
            {
                // Gun is not active, switch to the hands animator controller
                animator.runtimeAnimatorController = playerControllerHands;
            }
        }

        ReadPlayerStateAndAnimate();
    }

    /// <summary>
    /// Description:
    /// Reads the player's state and then sets and unsets booleans in the animator accordingly
    /// Input:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void ReadPlayerStateAndAnimate()
    {
        if (animator == null)
        {
            return;
        }

        ResetAnimations();

        // Shooting direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootingDirection = (mousePosition - transform.position).normalized;

         // Check movement states
        bool isMovingRight = rb.velocity.x > 0;
        bool isMovingLeft = rb.velocity.x < 0;

        // Calculate the angle in degrees
        float angle = Vector2.SignedAngle(Vector2.right, shootingDirection); // Use Vector2.right for rightward direction

        // Shooting animations
        if (Input.GetButton("Fire1"))
        {
            // Determine shooting direction based on angle
            if (angle >= 45 && angle < 135)
            {
                // Shooting upwards
                UpdateMovementAnimation();
            }
            else if (angle < -45 && angle > -135)
            {
                // Shooting downwards
                animator.SetBool("isShootingDown", true);
            }
            else if (angle >= -45 && angle < 45)
            {
                // Shooting right
                animator.SetBool("isShootingHorizontal", true); 
            }
            else if (angle < -135 || angle >= 135)
            {
                // Shooting left
                animator.SetBool("isShootingHorizontal", true); 
            }
        }
        else
        {
            // If not shooting, determine state based on player's movement
            UpdateMovementAnimation();
        }

        // Uncomment when adding the dead state
        // animator.SetBool("isDead", playerController.state == PlayerController.PlayerState.Dead);
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
        // Determine state based on player's movement
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
