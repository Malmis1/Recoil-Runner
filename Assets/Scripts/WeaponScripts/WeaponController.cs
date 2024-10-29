using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

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
    private int _totalAmmo;
    public int totalAmmo
    {
        get => _totalAmmo;
        set => _totalAmmo = value;
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        if (gun != null) {
            gunScript = gun.GetComponent<GunScript>();

            Transform weaponTransform = gun.transform.Find("Weapon");
            if (weaponTransform != null) {
            } else {
                Debug.LogError("Weapon child object not found on gun.");
            }

            // Find the MuzzleFlash in the "Weapon" child object
            Transform muzzleFlashTransform = weaponTransform?.Find("MuzzleFlash");
            if (muzzleFlashTransform != null) {
                gunScript.muzzleFlashParent = muzzleFlashTransform.gameObject;
            } else {
                Debug.LogError("MuzzleFlash child object not found under Weapon.");
            }
        } else {
            Debug.LogError("Gun object is not assigned in WeaponController.");
        }
    }
    
    private void Update() {
        HandleWeaponSwitching();

        if (!initialRecoil && playerController.GetIsGrounded()) {
            initialRecoil = true;
        }
    }

    public void ChangeGunByIndex(int gunIndex)
    {
        if (gunIndex >= 0 && gunIndex < gunDataList.Length)
        {
            gun.SetActive(true);  // Show the gun
            currentGunIndex = gunIndex;

            if (ammoCounter != null)
            {
                ammoCounter.SetActive(true);
            }
            else {
            Debug.LogWarning("AmmoCounter not found in WeaponController");
            }       

            // Apply the GunData to the GunScript
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
        gun.SetActive(false);  // Hide the gun
        currentGunIndex = -1;

         if (ammoCounter != null)
        {
            ammoCounter.SetActive(false);
        } 
        else {
            Debug.LogWarning("AmmoCounter not found in WeaponController");
        }
    }

     private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeactivateGun();  // No gun equipped
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeGunByIndex(0);  // Gun configuration 1
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeGunByIndex(1);  // Gun configuration 2
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

    public void ApplyRecoil(float recoilForce, float additiveRecoilAngleThreshold, bool initialRecoilResetsVelocity) {
        Vector2 recoilDirection = -gun.transform.up;
        if (initialRecoilResetsVelocity && initialRecoil) {
            ResetVelocity(); // Reset the velocity first so the recoil is consistent
            initialRecoil = false;
        }
        float angle = Vector2.Angle(recoilDirection, rb.velocity);
        if (angle >= additiveRecoilAngleThreshold / 2) {
            ResetVelocity();
        }
        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
    }

    public bool TryUseAmmo(int amount)
    {
        if (totalAmmo >= amount)
        {
            totalAmmo -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
