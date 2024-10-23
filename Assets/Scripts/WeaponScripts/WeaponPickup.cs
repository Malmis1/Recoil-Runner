using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Reference to the player object that has the WeaponController")]
    public GameObject player;

    [Tooltip("The index of the gun in WeaponController's gunDataList to equip")]
    public int gunIndex;

    [Tooltip("Hover height and speed")]
    public float hoverHeight = 0.5f;  
    public float hoverSpeed = 2f;     

    [Tooltip("Glow object behind the weapon pickup")]
    public SpriteRenderer glowSpriteRenderer;  

    [Tooltip("Glow pulse speed (how fast it fades in and out)")]
    public float glowPulseSpeed = 2f;

    private Vector3 initialPosition;
    private WeaponController weaponController;
    private Color originalGlowColor;

    private void Start()
    {
        // Get the WeaponController component from the player
        weaponController = player.GetComponent<WeaponController>();

        if (weaponController == null)
        {
            Debug.LogError("WeaponController component not found on the player object!");
        }

        // Store the initial position for hovering
        initialPosition = transform.position;

        // Store the initial color of the glow object
        if (glowSpriteRenderer != null)
        {
            originalGlowColor = glowSpriteRenderer.color;
        }
    }

    private void Update()
    {
        // Apply hovering effect by modifying the Y position using a sine wave
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

        // Apply the pulsing glow effect
        if (glowSpriteRenderer != null)
        {
            // PingPong oscillates between 0 and 1 over time, creating a smooth fade effect
            float glowAlpha = Mathf.PingPong(Time.time * glowPulseSpeed, 1f);
            Color glowColor = originalGlowColor;
            glowColor.a = glowAlpha;  // Change only the alpha (transparency) channel

            glowSpriteRenderer.color = glowColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player is colliding with the weapon pickup
        if (other.CompareTag("Player"))
        {
            // Equip the new weapon by calling ChangeGun on the WeaponController
            weaponController.ChangeGun(gunIndex);

            // Optionally, destroy the weapon pickup object after it's collected
            Destroy(gameObject);
        }
    }
}
