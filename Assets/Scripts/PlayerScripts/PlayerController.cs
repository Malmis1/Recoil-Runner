using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    [Tooltip("The force applied when jumping")]
    [SerializeField] private float jumpForce = 400f;

    [Tooltip("The amount to smooth out movement")]
    [Range(0, .5f)][SerializeField] private float movementSmoothing = .05f;

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
    private Vector3 velocity = Vector3.zero;
    private SpriteRenderer spriteRenderer;

    private float defaultMovementSmoothing;

    private Coroutine airControlCoroutine;

    [Tooltip("Maximum horizontal speed")]
    public float maxHorizontalSpeed = 10f;

    [Tooltip("Maximum vertical speed")]
    public float maxVerticalSpeed = 20f;
    public enum PlayerState
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dead
    }

    // The player's current state (walking, idle, jumping, or falling)
    public PlayerState state = PlayerState.Idle;

    private bool isJumping;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMovementSmoothing = movementSmoothing;
    }

    private void FixedUpdate() {
        CheckIfGrounded();
        RotateTowardsCursor();
        DetermineState();

        // Limit the player's velocity
        LimitVelocity();
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
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        if (isGrounded && !wasGrounded) {
            // Player has just landed
            RestoreFullAirControl();
        }
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
        Vector3 targetVelocity = new Vector2(move * moveSpeed * 10, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        // Jump
        if (isGrounded && jump) {
            isGrounded = false;
            isJumping = true; 
            rb.AddForce(new Vector2(0f, jumpForce));
        }
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

    public void AdjustAirControl(float recoilForce)
    {
        movementSmoothing = 0.5f;

        if (airControlCoroutine != null)
        {
            StopCoroutine(airControlCoroutine);
            airControlCoroutine = null;
        }
    }

    public void StartRestoreAirControl(float recoilForce)
    {
        if (airControlCoroutine != null)
        {
            StopCoroutine(airControlCoroutine);
        }
        airControlCoroutine = StartCoroutine(RestoreAirControl(recoilForce));
    }

    private IEnumerator RestoreAirControl(float recoilForce)
    {
        float duration = recoilForce / 4f; // How fast you get air control back (PS: Less value means longer)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (isGrounded)
            {
                RestoreFullAirControl();
                yield break;
            }

            movementSmoothing = Mathf.Lerp(0.5f, defaultMovementSmoothing, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movementSmoothing = defaultMovementSmoothing;
        airControlCoroutine = null;
    }

    private void RestoreFullAirControl()
    {
        movementSmoothing = defaultMovementSmoothing;

        if (airControlCoroutine != null)
        {
            StopCoroutine(airControlCoroutine);
            airControlCoroutine = null;
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
