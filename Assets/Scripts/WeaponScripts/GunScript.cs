using System.Collections;
using UnityEngine;

public class GunScript : MonoBehaviour {
    [Header("Controller")]
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Tooltip("Reference to the MuzzleFlash game object")]
    public GameObject muzzleFlashParent;

    private  SpriteRenderer gunSpriteRenderer;
    private ParticleSystem muzzleFlash;
    private float recoilForce;
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
    }
    
    void Update() {
        if (Time.timeScale == 0) {
            return;
        }

        Vector3 mouseDirection = Input.mousePosition;
        controller.LookAtPoint(mouseDirection);

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
    }

    public void ApplyGunData(GunData gunData) {
        // Set stats
        recoilForce = gunData.recoilForce;
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

        bool shouldFlip = mouseWorldPosition.x < controller.transform.position.x;

        gunSpriteRenderer.flipY = shouldFlip;

        isFlipped = shouldFlip;
    }
    
    private void Fire() { // Everything that should happen when player fires
        controller.ApplyRecoil(recoilForce, additiveRecoilAngleThreshold, initialRecoilResetsVelocity);
        nextFireTime = Time.time + fireRate;

        PlayMuzzleFlashEffect();

        FireAndShowEffects();

        isFiring = true;
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

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, controller.maxDistance, controller.hitLayers);

        Vector2 hitPoint;
        if (hit.collider != null) {
            hitPoint = hit.point;
            Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
        } else {
            hitPoint = origin + direction * controller.maxDistance; 
        }

        CreateBulletTrail(origin, hitPoint); // Create the bullet trail when firing
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
}
