using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("The name of the gun in WeaponController's gunDataList to equip")]
    public string gunName;

    [Tooltip("The audioclip for picking up weapon")]
    public AudioClip pickupSound;
    public int ammoAmount = 30;

    private void OnTriggerEnter2D(Collider2D other) {
        WeaponController weaponController = other.GetComponent<WeaponController>();
        if (weaponController != null) {
            if (pickupSound != null) {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            weaponController.ChangeGunByName(gunName, ammoAmount);
            Destroy(gameObject);
        }
    }
}
