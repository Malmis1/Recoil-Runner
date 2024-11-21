using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Tooltip("The amount of ammo to add")]
    public int ammoAmount = 10;

    [Tooltip("Check to enable respawning after pickup")]
    public bool respawn = false;

    [Tooltip("Respawn cooldown in seconds")]
    public float respawnCooldown = 5f;

    [Tooltip("The audioclip for picking up ammo")]
    public AudioClip pickupSound;

    private AudioSource audioSource;

    private BoxCollider2D pickupCollider;
    private SpriteRenderer[] renderers;
    private bool hasBeenCollected = false;

    private void Start()
    {
        pickupCollider = GetComponent<BoxCollider2D>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        WeaponController weaponController = other.GetComponent<WeaponController>();
        if (weaponController != null && !hasBeenCollected)
        {
            hasBeenCollected = true;

            if (weaponController.gun != null) {
                GunScript gunScript = weaponController.gun.GetComponent<GunScript>();
                if (gunScript != null) {
                    gunScript.currentAmmo += ammoAmount; // Add ammo to the gun
                } else {
                    Debug.LogWarning("GunScript not found on the gun object.");
                }
            } else {
                Debug.LogWarning("No gun assigned to WeaponController.");
            }

            audioSource.PlayOneShot(pickupSound);

            if (respawn)
            {
                DisablePickupVisuals();
                StartCoroutine(RespawnAfterCooldown());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private System.Collections.IEnumerator RespawnAfterCooldown()
    {
        yield return new WaitForSeconds(respawnCooldown);

        hasBeenCollected = false;
        EnablePickupVisuals();
    }

    private void DisablePickupVisuals()
    {
        pickupCollider.enabled = false;

        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    private void EnablePickupVisuals()
    {
        pickupCollider.enabled = true;

        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }
}
