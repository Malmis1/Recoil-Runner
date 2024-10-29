using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    [Tooltip("The force applied when jumping")]
    [SerializeField] private float jumpForce = 400f;

    [Tooltip("The layer to check for ground")]
    [SerializeField] private LayerMask whatIsGround;

    [Tooltip("The position to check if the enemy is grounded")]
    [SerializeField] private Transform groundCheck;

    [Tooltip("The horizontal range where enemy can detect player")]
    public float horizontalDetectionRange = 5f;

    [Tooltip("The vertical range where enemy can detect player")]
    public float verticalDetectionRange = 2f;

    [Tooltip("Player layer")]
    public LayerMask playerLayer;

    [Tooltip("Obstacles layers")]
    public LayerMask obstacleLayer;
    
    private const float groundedRadius = .2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;
    private Transform playerTransform;

    public EnemyState state = EnemyState.Idle;
    public UnityEvent OnLandEvent;

    private bool isJumping;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        OnLandEvent ??= new UnityEvent();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        CheckIfGrounded();
        RotateTowardsWalkingDirection();
        DetermineState();
    }

    private void CheckIfGrounded() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }

    public void Move(float move, bool jump) {
        // Run
        Vector3 targetVelocity = new Vector2(move * moveSpeed * 10, rb.velocity.y);
        rb.velocity = targetVelocity;

        // Jump
        if (isGrounded && jump) {
            isGrounded = false;
            isJumping = true; 
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip() {
        // Flip the character
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public float AdjustBulletSpeed(float originalShootSpeed) {
        if (!facingRight) {
            originalShootSpeed = originalShootSpeed * (-1);
        }

        return originalShootSpeed;
    }

    private void RotateTowardsWalkingDirection() {
        float xVel = rb.velocity.x;

        if (xVel < -0.001 && facingRight) {
            Flip();
        } else if (xVel > 0 && !facingRight) {
            Flip();
        }
    }

    public bool IsMoving() {
        float xVel = rb.velocity.x;
        bool isMoving = false;

        if (Mathf.Abs(xVel) > 0.01f) {
            isMoving = true;
        }

        return isMoving;
    }

    public void DetectPlayer() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            Vector2 directionToPlayer = player.transform.position - transform.position;

            if (Mathf.Abs(directionToPlayer.x) <= horizontalDetectionRange && Mathf.Abs(directionToPlayer.y) <= verticalDetectionRange) {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, horizontalDetectionRange, obstacleLayer | playerLayer);

                if (hit.collider != null && hit.collider.CompareTag("Player")) {
                    playerTransform = player.transform;

                    return;
                }
            }
        }

        playerTransform = null;
    }

    private void OnDrawGizmosSelected() { // Test method for visualizing the detection zone
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(horizontalDetectionRange * 2, verticalDetectionRange * 2, 0);
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

    public Transform getPlayerTransform() {
        return playerTransform;
    }

    public enum EnemyState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dead,
    }

    private void SetState(EnemyState newState)
    {
        state = newState;
    }

    private void DetermineState()
    {
        if (isGrounded)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.01f)
            {
                SetState(EnemyState.Walk);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else
        {
            if (isJumping)
            {
                if (rb.velocity.y <= 0)
                {
                    isJumping = false; 
                    SetState(EnemyState.Fall);
                }
                else
                {
                    SetState(EnemyState.Jump);
                }
            }
            else
            {
                SetState(EnemyState.Fall);
            }
        }
    }
}
