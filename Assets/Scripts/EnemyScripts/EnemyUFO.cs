using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(BoxCollider2D))]
public class EnemyUFO : MonoBehaviour
{
    public Transform player;
    public float hoverHeight = 5f;
    public float speed = 3f;
    public float stoppingDistance = 0.1f;
    public float laserLength = 10f;

    private Vector2 targetPosition;
    private LineRenderer lineRenderer;
    private BoxCollider2D laserCollider;

    void Start()
    {
        // Set up the LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        // Set up the BoxCollider2D and configure it as a trigger
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.isTrigger = true;
        laserCollider.enabled = false; // Disable initially

        // Tag this object as "Damage"
        gameObject.tag = "Damage";
    }

    void Update()
    {
        if (player != null)
        {
            targetPosition = new Vector2(player.position.x, player.position.y + hoverHeight);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            if (Vector2.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                transform.position += (Vector3)(direction * speed * Time.deltaTime);
                lineRenderer.enabled = false;
                laserCollider.enabled = false; // Disable the collider while moving
            }
            else
            {
                lineRenderer.enabled = true;
                laserCollider.enabled = true; // Enable the collider when shooting
                ShootLaser();
            }
        }
    }

    void ShootLaser()
    {
        // Set the laser's start and end positions
        Vector3 laserStart = transform.position;
        Vector3 laserEnd = transform.position - Vector3.up * laserLength;
        
        lineRenderer.SetPosition(0, laserStart);
        lineRenderer.SetPosition(1, laserEnd);

        // Adjust the BoxCollider2D to match the laser's position and length
        laserCollider.offset = new Vector2(0, -laserLength / 2); // Center it along the laser
        laserCollider.size = new Vector2(0.1f, laserLength); // Small width, full laser length
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Add collision logic here, e.g., checking if the laser hits the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by laser!");
            // Apply damage or other effects here
        }
    }
}
