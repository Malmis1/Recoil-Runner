using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour {
    [Tooltip("The controller for the weapon")]
    public WeaponController controller;

    [Tooltip("The time between shots (in seconds)")]
    public float fireRate = 1.0f;

    [Tooltip("Muzzleflash for the gun")]
    public ParticleSystem muzzleFlash;

    private float nextFireTime = 0f;

    void Update() {
        Vector3 mouseDirecton = Input.mousePosition;
        controller.LookAtPoint(mouseDirecton);

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime) { // Can also change the GetButtonDown to GetButton for auto fire
            controller.ApplyRecoil();
            nextFireTime = Time.time + fireRate;

            muzzleFlash.Play();
        }
    }
}
