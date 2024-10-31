using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GunScript : MonoBehaviour {
    [Header("Controller")]
    [Tooltip("The controller for the weapon")]
    public WeaponController weaponController;

    [Tooltip("Reference to the MuzzleFlash game object")]
    public GameObject muzzleFlashParent;

    [Tooltip("The audio source for playing gun sound effects")]
    public AudioSource audioSource;

    [HideInInspector] public TMP_Text currentAmmoText;
    [HideInInspector] public TMP_Text totalAmmoText;
    [HideInInspector] public int currentAmmo;
    private  SpriteRenderer gunSpriteRenderer;
    private ParticleSystem muzzleFlash;
    private float recoilForce;
    private int maxAmmo;
    private float fireRate;
    private float shotsPerFire;
    private float spreadAngle;
    private float additiveRecoilAngleThreshold;
    private bool initialRecoilResetsVelocity;
    private bool isAutomatic;
    private GameObject bulletTrailPrefab;
    private GameObject hitEffectPrefab;
    private float bulletTrailFadeDuration;
    private float nextFireTime = 0f;
    private bool isFlipped;
    private bool isFiring;
    private List<GameObject> activeBulletTrails = new List<GameObject>();   
    private AudioClip shootingClip;
    private AudioClip reloadClip;
    private float damage;

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

        audioSource = GetComponent<AudioSource>();
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

    public void ApplyGunData(GunData gunData)
    {
        recoilForce = gunData.recoilForce;
        maxAmmo = gunData.maxAmmo;
        fireRate = gunData.fireRate;
        shotsPerFire = gunData.shotsPerFire;
        spreadAngle = gunData.spreadAngle; 
        additiveRecoilAngleThreshold = gunData.additiveRecoilAngleThreshold;
        initialRecoilResetsVelocity = gunData.initialRecoilResetsVelocity;
        isAutomatic = gunData.isAutomatic;
        damage = gunData.damage;

        bulletTrailPrefab = gunData.bulletTrailPrefab;
        bulletTrailFadeDuration = gunData.bulletTrailFadeDuration;
        hitEffectPrefab = gunData.hitEffectPrefab;

        if (gunSpriteRenderer != null)
        {
            gunSpriteRenderer.sprite = gunData.gunSprite;
        }

        if (gunData.muzzleFlashPrefab != null)
        {
            ChangeMuzzleFlash(gunData.muzzleFlashPrefab, gunData.muzzleFlashOffset);
        }

        shootingClip = gunData.shootingAudio;
        reloadClip = gunData.reloadAudio;
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
    
    private void Fire() {
        if (currentAmmo <= 0) {
            return;
        }

        weaponController.ApplyRecoil(recoilForce, additiveRecoilAngleThreshold, initialRecoilResetsVelocity);
        nextFireTime = Time.time + fireRate;

        PlayMuzzleFlashEffect();
        FireAndShowEffects();

        if (shootingClip != null) {
            audioSource.clip = shootingClip;
            audioSource.Play();
        } else {
            Debug.LogWarning("Shooting clip is not assigned.");
        }

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
        for (int i = 0; i < shotsPerFire; i++) {
            float spread = Random.Range(-spreadAngle, spreadAngle);
            Vector2 direction = Quaternion.Euler(0, 0, spread) * transform.up;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, weaponController.maxDistance, weaponController.hitLayers);

            Vector2 hitPoint;
            if (hit.collider != null) {
                hitPoint = hit.point;
                Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
            } else {
                hitPoint = origin + direction * weaponController.maxDistance;
            }

            CreateBulletTrail(origin, hitPoint); // Create a bullet trail for each shot
        }
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

            if (reloadClip != null) {
                audioSource.clip = reloadClip;
                audioSource.Play();
            } else {
                Debug.LogWarning("Reload audio clip is not assigned.");
            }
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
        activeBulletTrails.Add(trail);

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

        activeBulletTrails.Remove(lineRenderer.gameObject);
        Destroy(lineRenderer.gameObject);
    }

    private void UpdateAmmoUI()
    {
        if (currentAmmoText != null)
        {
            currentAmmoText.text = currentAmmo.ToString();
        }
        else 
        {
            Debug.LogWarning("currentAmmoText is null");
        }

        if (totalAmmoText != null && weaponController != null)
        {
            totalAmmoText.text = weaponController.totalAmmo.ToString();
        } 
        else 
        {
            Debug.LogWarning("currentAmmoText or weaponController is null");
        }
    }

    private void OnDisable() {
        DestroyAllActiveBulletTrails();
    }

    private void DestroyAllActiveBulletTrails() {
        foreach (GameObject trail in activeBulletTrails) {
            if (trail != null) {
                Destroy(trail);
            }
        }
        activeBulletTrails.Clear(); 
    }
}
