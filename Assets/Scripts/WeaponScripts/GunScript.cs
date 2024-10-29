using System.Collections;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour {
    [Header("Controller")]
    [Tooltip("The controller for the weapon")]
    public WeaponController weaponController;

    [Tooltip("Reference to the MuzzleFlash game object")]
    public GameObject muzzleFlashParent;

    [Header("UI Elements")]
    [Tooltip("Parent GameObject containing the current ammo Text component")]
    public GameObject CurrentAmmoContainer;

    [Tooltip("Parent GameObject containing the total ammo Text component")]
    public GameObject TotalAmmoContainer;

    private  SpriteRenderer gunSpriteRenderer;
    private ParticleSystem muzzleFlash;
    private float recoilForce;
    private int currentAmmo;
    private int maxAmmo;
    private float fireRate;
    private float additiveRecoilAngleThreshold;
    private bool initialRecoilResetsVelocity;
    private bool isAutomatic;
    private GameObject bulletTrailPrefab;
    private GameObject hitEffectPrefab;
    private float bulletTrailFadeDuration;
    private float nextFireTime = 0f;
    private bool isFlipped;
    private bool isFiring;
    private TMP_Text currentAmmoText;
    private TMP_Text totalAmmoText;

    private void Awake() {
        Transform weaponTransform = transform.Find("Weapon");
        if (weaponTransform != null) {
            gunSpriteRenderer = weaponTransform.GetComponent<SpriteRenderer>();
            if (gunSpriteRenderer == null) {
                Debug.LogError("No SpriteRenderer found on Weapon child object.");
            }
        } else {
            Debug.LogError("Weapon child object not found.");
        }
        
        if (CurrentAmmoContainer != null)
        {
            currentAmmoText = CurrentAmmoContainer.GetComponentInChildren<TMP_Text>();
        }

        if (TotalAmmoContainer != null)
        {
            totalAmmoText = TotalAmmoContainer.GetComponentInChildren<TMP_Text>();
        }

        UpdateAmmoUI();
    }
    
    void Update() {
        if (Time.timeScale == 0) {
            return;
        }

        Vector3 mouseDirection = Input.mousePosition;
        weaponController.LookAtPoint(mouseDirection);

        FlipGunSprite();

        isFiring = false;

        if (isAutomatic) {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime) {
                Fire();
                isFiring = true;
            }
        } else {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) {
                Fire();
                isFiring = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        UpdateAmmoUI();
    }

    public void ApplyGunData(GunData gunData) {
        // Set stats
        recoilForce = gunData.recoilForce;
        maxAmmo = gunData.maxAmmo;
        currentAmmo = maxAmmo;
        fireRate = gunData.fireRate;
        additiveRecoilAngleThreshold = gunData.additiveRecoilAngleThreshold;
        initialRecoilResetsVelocity = gunData.initialRecoilResetsVelocity;
        isAutomatic = gunData.isAutomatic;

        bulletTrailPrefab = gunData.bulletTrailPrefab;
        bulletTrailFadeDuration = gunData.bulletTrailFadeDuration;

        hitEffectPrefab = gunData.hitEffectPrefab;

        // Set sprite
        if (gunSpriteRenderer != null) {
            gunSpriteRenderer.sprite = gunData.gunSprite;
        }

        // Instantiate the muzzle flash prefab
        if (gunData.muzzleFlashPrefab != null) {
            ChangeMuzzleFlash(gunData.muzzleFlashPrefab, gunData.muzzleFlashOffset);
        }
    }

    private void ChangeMuzzleFlash(GameObject newMuzzleFlashPrefab, Vector3 muzzleFlashOffset) {
        if (muzzleFlash != null)
        {
            Destroy(muzzleFlash.gameObject); 
        }

        if (newMuzzleFlashPrefab != null)
        {
            GameObject newMuzzleFlashInstance = Instantiate(newMuzzleFlashPrefab, muzzleFlashParent.transform);
        
            newMuzzleFlashInstance.transform.localPosition = muzzleFlashOffset;
            muzzleFlash = newMuzzleFlashInstance.GetComponent<ParticleSystem>();
        }
        else
        {
            Debug.LogError("Muzzle flash prefab is null.");
        }
    }
    private void FlipGunSprite() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool shouldFlip = mouseWorldPosition.x < weaponController.transform.position.x;

        gunSpriteRenderer.flipY = shouldFlip;

        isFlipped = shouldFlip;
    }
    
    private void Fire() { // Everything that should happen when player fires
        if (currentAmmo <= 0)
        {
            return; // No ammo
        }

        weaponController.ApplyRecoil(recoilForce, additiveRecoilAngleThreshold, initialRecoilResetsVelocity);
        nextFireTime = Time.time + fireRate;

        PlayMuzzleFlashEffect();

        FireAndShowEffects();

        currentAmmo--; 
        isFiring = true;

        UpdateAmmoUI();
    }

    public bool IsFiring() {
        return isFiring;  
    }

    public void PlayMuzzleFlashEffect() {
        if (muzzleFlash != null) {
            muzzleFlash.Play();
        }
        else {
            Debug.LogError("No ParticleSystem found on the current gun's muzzle flash.");
        }
    }

    private void FireAndShowEffects() {
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, weaponController.maxDistance, weaponController.hitLayers);

        Vector2 hitPoint;
        if (hit.collider != null) {
            hitPoint = hit.point;
            Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
        } else {
            hitPoint = origin + direction * weaponController.maxDistance; 
        }

        CreateBulletTrail(origin, hitPoint); // Create the bullet trail when firing
    }

    private void Reload()
    {
        if (currentAmmo == maxAmmo)
        {
            return;
        }

        int ammoNeeded = maxAmmo - currentAmmo; 
        if (weaponController.TryUseAmmo(ammoNeeded))
        {
            currentAmmo = maxAmmo; 
        }
        else
        {
            int ammoAvailable = weaponController.totalAmmo;
            currentAmmo += ammoAvailable;
            weaponController.TryUseAmmo(ammoAvailable);
        }

        UpdateAmmoUI();
    }

    private void CreateBulletTrail(Vector2 start, Vector2 end) {
        if (bulletTrailPrefab == null) {
            Debug.LogWarning("No bullet trail prefab assigned.");
            return;
        }

        GameObject trail = Instantiate(bulletTrailPrefab, start, Quaternion.identity);

        LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();
        if (lineRenderer != null) {
            lineRenderer.SetPosition(0, start); 
            lineRenderer.SetPosition(1, end);   

            StartCoroutine(FadeBulletTrail(lineRenderer, bulletTrailFadeDuration));
        }
    }

    private IEnumerator FadeBulletTrail(LineRenderer lineRenderer, float fadeDuration) {
        float timeElapsed = 0f;

        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        while (timeElapsed < fadeDuration) {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            lineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            lineRenderer.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);
            yield return null;
        }

        Destroy(lineRenderer.gameObject); // Destroy the trail after it fades out
    }

    private void UpdateAmmoUI()
    {
        print(currentAmmoText);
        if (currentAmmoText != null)
        {
            currentAmmoText.text = currentAmmo.ToString();
        }

        if (totalAmmoText != null && weaponController != null)
        {
            totalAmmoText.text = weaponController.totalAmmo.ToString();
        }
    }
}
