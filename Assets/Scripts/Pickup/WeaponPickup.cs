using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Reference to the player object that has the WeaponController")]
    public GameObject player;

    [Tooltip("The name of the gun in WeaponController's gunDataList to equip")]
    public string gunName;

    private WeaponController weaponController;

    private void Start()
    {
        weaponController = player.GetComponent<WeaponController>();
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
