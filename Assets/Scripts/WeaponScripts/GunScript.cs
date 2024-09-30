using UnityEngine;

public class GunScript : MonoBehaviour {
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Tooltip("The time between shots (in seconds)")]
    public float fireRate = 1.0f;

    [Tooltip("Muzzleflash for the gun")]
    public ParticleSystem muzzleFlash;

    private float nextFireTime = 0f;

    void Update() {
        Vector3 mouseDirection = Input.mousePosition;
        controller.LookAtPoint(mouseDirection);

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) {
            controller.ApplyRecoil();
            nextFireTime = Time.time + fireRate;

            muzzleFlash.Play();

            controller.sendRayCastAndPlayHitEffect();
        }
    }
}
