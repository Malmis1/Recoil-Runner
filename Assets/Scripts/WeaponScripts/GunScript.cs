using UnityEngine;

public class GunScript : MonoBehaviour {
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Tooltip("Muzzleflash for the gun")]
    public ParticleSystem muzzleFlash;

    [Tooltip("The sprite renderer for the gun")]
    public SpriteRenderer gunSpriteRenderer;

    [Tooltip("The force/recoil which should be applied to player when weapon is fired")]
    public float recoilForce = 15f;

    [Tooltip("The time between shots (in seconds)")]
    public float fireRate = 1.0f;

    [Tooltip("If the weapon is automatic or not")]
    public bool isAutomatic = false;

    private float nextFireTime = 0f;

    void Update() {
        Vector3 mouseDirection = Input.mousePosition;
        controller.LookAtPoint(mouseDirection);

        FlipGunSprite();

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && isAutomatic) { // GetButton for automatic firing
            Fire();
        } 
        else if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !isAutomatic) {
            Fire();
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
    }

    public void FlipSpriteY(SpriteRenderer spriteRenderer) {
        spriteRenderer.flipY = !spriteRenderer.flipY;
    }
}
