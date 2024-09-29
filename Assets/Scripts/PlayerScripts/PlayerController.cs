using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The force applied when jumping")]
    [SerializeField] private float jumpForce = 400f;

    [Tooltip("The amount to smooth out movement")]
    [Range(0, .3f)][SerializeField] private float movementSmoothing = .05f;

    [Tooltip("Enable/disable air control")]
    [SerializeField] private bool airControl = true; // Removed the option to disable air control, set it as true by default

    [Tooltip("The layer to check for ground")]
    [SerializeField] private LayerMask whatIsGround;

    [Tooltip("The position to check if the player is grounded")]
    [SerializeField] private Transform groundCheck;

    private const float groundedRadius = .2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 velocity = Vector3.zero;

    public UnityEvent OnLandEvent;

    private void Awake() { // Awake instead of Start because it is initializing components before the game is starting.
        rb = GetComponent<Rigidbody2D>();
        OnLandEvent ??= new UnityEvent();
    }

    private void FixedUpdate() {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);

        if (isGrounded && !wasGrounded) {
            OnLandEvent.Invoke();
        }
    }

    public void Move(float move, bool jump) {
        if (isGrounded || airControl) {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (move > 0 && !facingRight) Flip();
            else if (move < 0 && facingRight) Flip();
        }

        if (isGrounded && jump) { // Maybe make holding space jump higher
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}