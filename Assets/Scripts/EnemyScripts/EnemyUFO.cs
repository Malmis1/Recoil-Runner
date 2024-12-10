using UnityEngine;

public class EnemyUFO : MonoBehaviour
{
    public Transform player;
    public float hoverHeight = 5f;
    public float speed = 3f;
    public float stoppingDistance = 0.5f;
    public float hoverTimeToFire = 0.2f;
    public float hoverWidth = 2f;

    private Vector2 targetPosition;
    public Laser laser;

    private float hoverTimer = 0f;

    void Start()
    {
        if (laser != null)
        {
            laser.DisableLaser();
        }
        else
        {
            Debug.LogWarning("Laser script not found on child object.");
        }
    }

    void Update()
    {
        if (player == null) return;

        targetPosition = new Vector2(player.position.x, player.position.y + hoverHeight);

        float distance = Vector2.Distance(transform.position, targetPosition);
        if (distance > stoppingDistance)
        {
            MoveTowardsTarget();
        }
        else
        {
            CheckHoverConditions();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        hoverTimer = 0f;
        laser?.DisableLaser();
    }

    private void CheckHoverConditions()
    {
        float horizontalDifference = Mathf.Abs(transform.position.x - player.position.x);
        bool withinWidth = horizontalDifference <= (hoverWidth / 2);

        float verticalDifference = Mathf.Abs((player.position.y + hoverHeight) - transform.position.y);
        bool verticallyAligned = verticalDifference < stoppingDistance;

        if (withinWidth && verticallyAligned)
        {
            hoverTimer += Time.deltaTime;

            if (hoverTimer >= hoverTimeToFire)
            {
                laser?.EnableLaser();
            }
        }
        else
        {
            hoverTimer = 0f;
            laser?.DisableLaser();
        }
    }
}
