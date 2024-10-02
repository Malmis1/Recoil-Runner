using UnityEngine;

public class GunScript : MonoBehaviour {
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Tooltip("Muzzleflash for the gun")]
    public ParticleSystem muzzleFlash;

    [Tooltip("The sprite renderer for the gun")]
    public SpriteRenderer gunSpriteRenderer;

    [Tooltip("The time between shots (in seconds)")]
    public float fireRate = 1.0f;

    private float nextFireTime = 0f;

    void Update() {
        Vector3 mouseDirection = Input.mousePosition;
        controller.LookAtPoint(mouseDirection);

        FlipGunSprite();

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) {
            controller.ApplyRecoil();
            nextFireTime = Time.time + fireRate;

            muzzleFlash.Play();

            controller.sendRayCastAndPlayHitEffect();
        }
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
