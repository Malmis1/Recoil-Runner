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

    void Start() {
        ChangeToWeapon1();
    }

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

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeToWeapon1();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeToWeapon2();
        }
    }

    private void Fire() { // Everything that should happen when player fires
        controller.ApplyRecoil(recoilForce);
        nextFireTime = Time.time + fireRate;

        controller.PlayMuzzleFlashEffect();

        controller.SendRayCastAndPlayHitEffect();
    }

    private void FlipGunSprite() {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        bool shouldFlip = mouseWorldPosition.x < controller.transform.position.x;

        gunSpriteRenderer.flipY = shouldFlip;

        isFlipped = shouldFlip;
    }

    private void ChangeToWeapon1() {
        controller.ChangeToWeaponVisuals1(gunSpriteRenderer, muzzleFlashParent);

        recoilForce = 15f;
        fireRate = 0.5f;
        isAutomatic = false;
    }

    private void ChangeToWeapon2() {
        controller.ChangeToWeaponVisuals2(gunSpriteRenderer, muzzleFlashParent);

        recoilForce = 5f;
        fireRate = 0.2f;
        isAutomatic = true;
    }

}
