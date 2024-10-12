using UnityEngine;

public class GunScript : MonoBehaviour {
    [Header("Controller")]
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Header("Muzzle flash")]
    [Tooltip("Muzzleflash for the gun")]
    public ParticleSystem muzzleFlash;

    [Tooltip("Reference to the MuzzleFlash game object")]
    public GameObject muzzleFlashParent;

    [Header("Sprite renderer")]
    [Tooltip("The sprite renderer for the gun")]
    public SpriteRenderer gunSpriteRenderer;

    [Header("Gun settings")]
    [Tooltip("The force/recoil which should be applied to player when weapon is fired")]
    public float recoilForce = 15f;

    [Tooltip("The time between shots (in seconds)")]
    public float fireRate = 1.0f;

    [Tooltip("If the weapon is automatic or not")]
    public bool isAutomatic = false;

    private float nextFireTime = 0f;

    private bool isFlipped;

    void Update() {
        Vector3 mouseDirection = Input.mousePosition;
        controller.LookAtPoint(mouseDirection);

        FlipGunSprite();

        if (isAutomatic) {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime) {
                Fire();
            }
        } else {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) {
                Fire();
            }
        }
    }

    private void Fire() { // Everything that should happen when player fires
        controller.ApplyRecoil(recoilForce);
        nextFireTime = Time.time + fireRate;

        muzzleFlash.Play();

        controller.sendRayCastAndPlayHitEffect();
    }

    private void FlipGunSprite() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool shouldFlip = mouseWorldPosition.x < controller.transform.position.x;

        gunSpriteRenderer.flipY = shouldFlip;

        isFlipped = shouldFlip;
    }
}
