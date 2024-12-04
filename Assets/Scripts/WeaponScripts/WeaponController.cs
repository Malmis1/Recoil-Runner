using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    [Tooltip("List of GunData configurations")]
    public GunData[] gunDataList;

    [Tooltip("Maximum distance for the raycast")]
    public float maxDistance = 100f;

    [Tooltip("Layers to detect with the raycast")]
    public LayerMask hitLayers;
    [HideInInspector] public UnityEngine.UI.Image hudImage;
    [HideInInspector] public GameObject ammoCounter;
    private int currentGunIndex = -1;
    private GunScript gunScript;
    private Rigidbody2D rb;
    private bool initialRecoil = true;
    private PlayerController playerController;
    private int starterAmmo;
    private Dictionary<int, int> gunAmmoDict = new Dictionary<int, int>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        if (gun != null)
        {
            gunScript = gun.GetComponent<GunScript>();

            Transform weaponTransform = gun.transform.Find("Weapon");
            if (weaponTransform != null)
            {
            }
            else
            {
                Debug.LogError("Weapon child object not found on gun.");
            }

            // Find the MuzzleFlash in the "Weapon" child object
            Transform muzzleFlashTransform = weaponTransform?.Find("MuzzleFlash");
            if (muzzleFlashTransform != null)
            {
                gunScript.muzzleFlashParent = muzzleFlashTransform.gameObject;
            }
            else
            {
                Debug.LogError("MuzzleFlash child object not found under Weapon.");
            }
        }
        else
        {
            Debug.LogError("Gun object is not assigned in WeaponController.");
        }
    }

    private void Update()
    {
        if (!initialRecoil && playerController.GetIsGrounded())
        {
            initialRecoil = true;
        }
    }

    public void ChangeGunByIndex(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < gunDataList.Length)
        {

            if (currentGunIndex != -1 && gunScript != null)
            {
                gunAmmoDict[currentGunIndex] = gunScript.currentAmmo;
            }

            gun.SetActive(true);
            currentGunIndex = gunIndex;

            if (ammoCounter != null)
            {
                ammoCounter.SetActive(true);
            }
            else
            {
                Debug.LogWarning("AmmoCounter not found in WeaponController");
            }

            if (gunAmmoDict.TryGetValue(gunIndex, out int savedAmmo))
            {
                gunScript.currentAmmo = savedAmmo; // Restore saved ammo
            }
            else
            {
                gunScript.currentAmmo = starterAmmo; // Initialize with ammo
                gunAmmoDict[gunIndex] = gunScript.currentAmmo; // Save initial ammo
            }

            gunScript.ApplyGunData(gunDataList[gunIndex]);


            if (hudImage != null && gunDataList[gunIndex].hudSprite != null)
            {
                hudImage.sprite = gunDataList[gunIndex].hudSprite;
            }
            else
            {
                if (hudImage = null)
                {
                    Debug.LogWarning("hudImage or the is null");
                }

                if (gunDataList[gunIndex].hudSprite = null)
                {
                    Debug.LogWarning("hudSprite in gundata is null");
                }
            }
        }
        else
        {
            Debug.LogError("Invalid gun index");
        }
    }

    public void SetStarterAmmo(int ammo)
    {
        starterAmmo = ammo;
    }

    public void ChangeGunByName(string gunName)
    {
        int gunIndex = System.Array.FindIndex(gunDataList, g => g.name == gunName);
        if (gunIndex != -1)
        {
            ChangeGunByIndex(gunIndex); // Change to the gun if it’s found by name
        }
        else
        {
            Debug.LogWarning("Gun with name '" + gunName + "' not found in gunDataList.");
        }
    }

    public void DeactivateGun()
    {
        if (currentGunIndex != -1)
        {
            // Save current ammo before deactivating
            gunAmmoDict[currentGunIndex] = gunScript.currentAmmo;
        }

        gun.SetActive(false);  // Hide the gun
        currentGunIndex = -1;

        if (ammoCounter != null)
        {
            ammoCounter.SetActive(false);
        }
        else
        {
            Debug.LogWarning("AmmoCounter not found in WeaponController");
        }
    }

    public void LookAtPoint(Vector3 point)
    { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }

    private void ResetVelocity()
    {
        // rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y * 0); // can individually change the velocity for the axes
    }

    public void ApplyRecoil(float recoilForce, float additiveRecoilAngleThreshold, bool initialRecoilResetsVelocity)
    {
        Vector2 recoilDirection = -gun.transform.up;
        if (initialRecoilResetsVelocity && initialRecoil)
        {
            ResetVelocity(); // Reset the velocity first so the recoil is consistent
            initialRecoil = false;
        }
        float angle = Vector2.Angle(recoilDirection, rb.velocity);
        if (angle >= additiveRecoilAngleThreshold / 2)
        {
            ResetVelocity();
        }
        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
    }
}
