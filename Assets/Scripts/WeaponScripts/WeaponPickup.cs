using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Reference to the player object that has the WeaponController")]
    public GameObject player;

    [Tooltip("The name of the gun in WeaponController's gunDataList to equip")]
    public string gunName;

    [Tooltip("Hover height and speed")]
    public float hoverHeight = 0.5f;  
    public float hoverSpeed = 2f;     

    [Tooltip("Glow object behind the weapon pickup")]
    public SpriteRenderer glowSpriteRenderer;  
    [Tooltip("Rotation speed for the glow effect")]
    public float glowRotationSpeed = 100f;  
    [Tooltip("Scale speed for the glow effect")]
    public float glowScaleSpeed = 2f;  
    [Tooltip("Minimum and maximum scale for the glow effect")]
    public float minGlowScale = 0.08f;
    public float maxGlowScale = 0.15f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private WeaponController weaponController;

    private void Start()
    {
        weaponController = player.GetComponent<WeaponController>();
        initialPosition = transform.position;
        initialScale = glowSpriteRenderer.transform.localScale;
    }

    private void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

       if (glowSpriteRenderer != null)
        {
            glowSpriteRenderer.transform.Rotate(Vector3.forward, glowRotationSpeed * Time.deltaTime);

            float scale = Mathf.Lerp(minGlowScale, maxGlowScale, (Mathf.Sin(Time.time * glowScaleSpeed) + 1) / 2);
            glowSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            weaponController.ChangeGunByName(gunName);
            Destroy(gameObject);
        }
    }
}
