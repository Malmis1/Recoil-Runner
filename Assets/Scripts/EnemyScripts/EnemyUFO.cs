using UnityEngine;

public class EnemyUFO : MonoBehaviour
{
    public Transform player;
    public float hoverHeight = 5f;
    public float speed = 3f;
    public float stoppingDistance = 0.1f;

    private Vector2 targetPosition;
    private Laser laser; // Reference to the Laser script

    void Start() {
        laser = GetComponentInChildren<Laser>();
        if (laser != null) {
            laser.DisableLaser();
        }
        else {
            Debug.LogWarning("Laser script not found on child object.");
        }
    }

    void Update() {
        if (player != null) {
            targetPosition = new Vector2(player.position.x, player.position.y + hoverHeight);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            if (Vector2.Distance(transform.position, targetPosition) > stoppingDistance) {
                transform.position += (Vector3)(direction * speed * Time.deltaTime);
                laser?.DisableLaser();
            }
            else {
                laser?.EnableLaser();
            }
        }
    }
}
