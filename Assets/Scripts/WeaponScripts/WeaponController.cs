using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    [Tooltip("Particle effect to play at the hit point")]
    public GameObject hitEffectPrefab;

    [Tooltip("Maximum distance for the raycast")]
    public float maxDistance = 100f;

    [Tooltip("Layers to detect with the raycast")]
    public LayerMask hitLayers;

    [Tooltip("The angle in which the recoil will be added to the velocity instead of resetting it")]
    public float additiveRecoilAngleThreshold = 250f;

    private Rigidbody2D rb;
    private bool initialRecoil = true;
    private PlayerController playerController;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }
    
    private void Update() {
        if (!initialRecoil && playerController.GetIsGrounded()) {
            initialRecoil = true;
        }
    }

    public void LookAtPoint(Vector3 point) { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }

    private void ResetVelocity() {
        // rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y * 0); // can individually change the velocity for the axes
    }

    public void ApplyRecoil(float recoilForce) {
        Vector2 recoilDirection = -gun.transform.up;
        if (initialRecoil) {
            ResetVelocity(); // Reset the velocity first so the recoil is consistent
            initialRecoil = false;
        }
        float angle = Vector2.Angle(recoilDirection, rb.velocity);
        if (angle < additiveRecoilAngleThreshold / 2) {
            rb.velocity += recoilDirection * recoilForce;
        } else {
            ResetVelocity();
            rb.velocity = recoilDirection * recoilForce;
        }
    }

    public void sendRayCastAndPlayHitEffect() {
        Vector2 origin = gun.transform.position;
        Vector2 direction = gun.transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, hitLayers);

        if (hit.collider != null) {
            Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
        }
    }
}
