using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // Movement speed
    public float jumpForce = 7f;     // Force applied when jumping
    public Transform groundCheck;    // Position to check if the player is grounded
    public LayerMask groundLayer;    // Layer to check for ground

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