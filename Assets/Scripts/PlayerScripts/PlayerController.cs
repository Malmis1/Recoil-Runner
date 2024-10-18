using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    [Tooltip("Leniency for jumping after not being grounded anymore")]
    public float jumpGraceTime = 0.1f;
    private float timeToStopJumpGrace = 0;

    [Tooltip("The force applied when jumping")]
    [SerializeField] private float jumpForce = 400f;

    [Tooltip("The layer to check for ground")]
    [SerializeField] private LayerMask whatIsGround;

    [Tooltip("The position to check if the player is grounded")]
    [SerializeField] private Transform groundCheck;
    
    [Tooltip("The player's eyes")]
    [SerializeField] private Transform eyes;

    private const float groundedRadius = .2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private SpriteRenderer spriteRenderer;

    [Tooltip("Maximum horizontal speed")]
    public float maxHorizontalSpeed = 10f;

    [Tooltip("Maximum vertical speed")]
    public float maxVerticalSpeed = 20f;

    [Tooltip("Maximum acceleration of the player")]
    public float maxAccel = 35f;
    [Tooltip("The deceleration when the player moves")]
    public float maxControlledDecel = 35f;
    [Tooltip("The deceleration while the player is not moving")]
    // Higher values will result in air drag which causes the player to slow down in the air
    public float maxDefaultDecel = 5f;
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dead,
    }

    // The player's current state (walking, idle, jumping, or falling)
    public PlayerState state = PlayerState.Idle;

    private bool isJumping;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        CheckIfGrounded();
        RotateTowardsCursor();
        DetermineState();

        // Limit the player's velocity
        //LimitVelocity();
    }

    private void LimitVelocity() {
        Vector2 clampedVelocity = rb.velocity;

        // Limit horizontal speed
        if (Mathf.Abs(clampedVelocity.x) > maxHorizontalSpeed) {
            clampedVelocity.x = Mathf.Sign(clampedVelocity.x) * maxHorizontalSpeed;
        }

        // Limit vertical speed
        if (Mathf.Abs(clampedVelocity.y) > maxVerticalSpeed) {
            clampedVelocity.y = Mathf.Sign(clampedVelocity.y) * maxVerticalSpeed;
        }

        rb.velocity = clampedVelocity;
    }

    private void CheckIfGrounded() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
    }

    /// <summary>
    /// Gets whether or not the player is grounded.
    /// </summary>
    /// <returns><b>true</b> if the player is grounded.</returns>
    public bool GetIsGrounded() {
        return isGrounded;
    }

    public void Move(float move, bool jump) {
        // Run
        MoveH(move);

        // Jump
        if (jump && (isGrounded || Time.time < timeToStopJumpGrace)) {
            isGrounded = false;
            isJumping = true; 
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void MoveH(float move) {
        Vector2 targetVelocity = new Vector2(move * moveSpeed * 10, 0);

        float maxDecel = move != 0 ? maxControlledDecel : maxDefaultDecel;
        // The deceleration when the player moves in the opposite direction

        Vector2 deltaV = targetVelocity - rb.velocity;
        Vector2 accel = deltaV / Time.deltaTime;
        float limit = Vector2.Dot(deltaV, rb.velocity) > 0f ? maxAccel : maxDecel;
        Vector2 force = rb.mass * Vector2.ClampMagnitude(accel, limit);

        rb.AddForce(new Vector2(force.x, 0), ForceMode2D.Force);
    }

    private void RotateTowardsCursor() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 

        Vector3 direction = mousePosition - transform.position;

        // Flip based on the direction of the mouse
        if (direction.x < 0 && facingRight) {
            Flip();
        }
        else if (direction.x > 0 && !facingRight) {
            Flip();
        }
    }

    private void Flip() {
        // Flip the character
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;

        if (eyes != null) {
            Vector3 eyesPosition = eyes.localPosition;
            if (facingRight) {
                eyesPosition.x = Mathf.Abs(eyesPosition.x);
            } else {
                eyesPosition.x = -Mathf.Abs(eyesPosition.x);
            }
            eyes.localPosition = eyesPosition;
        }
    }

    private void SetState(PlayerState newState)
    {
        state = newState;
    }

    private void DetermineState()
    {
        if (isGrounded)
        {
            timeToStopJumpGrace = Time.time + jumpGraceTime;
            if (Mathf.Abs(rb.velocity.x) > 0.01f)
            {
                SetState(PlayerState.Walk);
            }
            else
            {
                SetState(PlayerState.Idle);
            }
        }
        else
        {
            if (isJumping)
            {
                if (rb.velocity.y <= 0)
                {
                    isJumping = false; 
                    SetState(PlayerState.Fall);
                }
                else
                {
                    SetState(PlayerState.Jump);
                }
            }
            else
            {
                SetState(PlayerState.Fall);
            }
        }
    }
}
