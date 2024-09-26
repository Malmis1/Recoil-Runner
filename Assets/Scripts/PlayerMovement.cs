using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The movement speed")]
    public float moveSpeed = 5f;
    [Tooltip("The force applied when jumping")]
    public float jumpForce = 7f;
    [Tooltip("The position to check if the player is grounded")]
    public Transform groundCheck;
    [Tooltip("The layer to check for ground")]
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get Rigidbody2D component
    }

    void Update()
    {
        // Handle horizontal movement
        float moveX = Input.GetAxis("Horizontal");  // Get input from left/right keys or A/D
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Apply jump force
        }
    }
}