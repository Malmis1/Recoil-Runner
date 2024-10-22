using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    [Tooltip("The force applied when jumping")]
    [SerializeField] private float jumpForce = 400f;

    [Tooltip("The amount to smooth out movement")]
    [Range(0, .5f)][SerializeField] private float movementSmoothing = .05f;

    [Tooltip("The layer to check for ground")]
    [SerializeField] private LayerMask whatIsGround;

    [Tooltip("The position to check if the enemy is grounded")]
    [SerializeField] private Transform groundCheck;

    [Tooltip("The amount of seconds the ground check is delayed after shooting")]
    [SerializeField] public float groundCheckDelay = 0.5f;
    
    private const float groundedRadius = .2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    private Coroutine airControlCoroutine;

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
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        if (isGrounded && !wasGrounded) {
            OnLandEvent.Invoke(); // Enemy landed
        }
    }

    public void Move(float move, bool jump) {
        // Run
        Vector3 targetVelocity = new Vector2(move * moveSpeed * 10, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

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

        if (xVel < -0.001 || xVel > 0) {
            isMoving = true;
        }

        return isMoving;
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
