using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    [Tooltip("Particle effect to play at the hit point")]
    public GameObject hitEffectPrefab;

    [Tooltip("Maximum distance for the raycast")]
    public float maxDistance = 100f;

    [Tooltip("Layers to detect with the raycast")]
    public LayerMask hitLayers;

    private Rigidbody2D rb;

    private GameObject currentMuzzleFlashInstance;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LookAtPoint(Vector3 point) { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }

    private void ResetVelocity() {
        // rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(rb.velocity.x * 0, rb.velocity.y * 0); // can individually change the velocity for the axes

    }

    public void ApplyRecoil(float recoilForce) {
        ResetVelocity(); // Reset the velocity first so the recoil is consistent

        Vector2 recoilDirection = -gun.transform.up;

        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
    }

    public void SendRayCastAndPlayHitEffect() {
        Vector2 origin = gun.transform.position;
        Vector2 direction = gun.transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, hitLayers);

        if (hit.collider != null) {
            Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
        }
    }

    public void ChangeWeaponSprite(SpriteRenderer spriteRenderer, string weaponSpritePath) {
        Sprite changedSprite = Resources.Load<Sprite>(weaponSpritePath);

        if (changedSprite != null) {
            spriteRenderer.sprite = changedSprite;
        } else {
            Debug.LogError("Sprite not found at the specified path.");
        }
    }

    public void ChangeMuzzleFlash(GameObject muzzleFlashParent, string muzzleFlashPath) {
        if (currentMuzzleFlashInstance != null) {
            Destroy(currentMuzzleFlashInstance);
        }

        GameObject newMuzzleFlashPrefab = Resources.Load<GameObject>(muzzleFlashPath);

        if (newMuzzleFlashPrefab != null) {
            currentMuzzleFlashInstance = Instantiate(newMuzzleFlashPrefab, muzzleFlashParent.transform);

            currentMuzzleFlashInstance.transform.localPosition = Vector3.zero;
            currentMuzzleFlashInstance.transform.localRotation = Quaternion.identity;
            currentMuzzleFlashInstance.transform.localScale = Vector3.one;
        }
        else {
            Debug.LogError("Failed to load muzzle flash prefab from path: " + muzzleFlashPath);
        }
    }

    public void ChangeToWeaponVisuals1(SpriteRenderer spriteRenderer, GameObject muzzleFlashParent) {
        ChangeWeaponSprite(spriteRenderer, "Art/Weapons/Weapon1");
        ChangeMuzzleFlash(muzzleFlashParent, "ParticleSystems/MuzzleFlash/MuzzleFlashRailgun");
    }

    public void ChangeToWeaponVisuals2(SpriteRenderer spriteRenderer, GameObject muzzleFlashParent) {
        ChangeWeaponSprite(spriteRenderer, "Art/Weapons/SimpleWeapon");
        ChangeMuzzleFlash(muzzleFlashParent, "ParticleSystems/MuzzleFlash/MuzzleFlashSimpleWeapon");
    }

    public void PlayMuzzleFlashEffect() {
    if (currentMuzzleFlashInstance != null) {
        ParticleSystem particleSystem = currentMuzzleFlashInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null) {
            particleSystem.Play();
        }
        else {
            Debug.LogError("No ParticleSystem found on the current muzzle flash instance.");
        }
    }
    else {
        Debug.LogError("No current muzzle flash instance to play.");
    }
}

}
