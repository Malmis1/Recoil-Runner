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

    [Header("Animator")]
    [Tooltip("Animator component on the player")]
    public Animator playerAnimator;

    [Tooltip("Animator controller with hands")]
    public RuntimeAnimatorController handsController;

    [Tooltip("Animator controller without hands")]
    public RuntimeAnimatorController noHandsController;

    private Rigidbody2D rb;

    [Header("Sprite Renderer")]
    public SpriteRenderer gunSpriteRenderer;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ShowWeapon(false); 
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ShowWeapon(true); 
            ChangeWeaponProperties(15f, 0.5f, false); 
            changeToWeaponSprite1(gunSpriteRenderer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ShowWeapon(true); 
            ChangeWeaponProperties(5f, 0.2f, true); 
            changeToWeaponSprite2(gunSpriteRenderer);
        }
    }

    public void ShowWeapon(bool show) {
        gun.SetActive(show); 

        if (show) {
            playerAnimator.runtimeAnimatorController = noHandsController; 
        } else {
            playerAnimator.runtimeAnimatorController = handsController; 
        }
    }

    public void ChangeWeaponProperties(float recoil, float fireRate, bool isAutomatic) {
        GunScript gunScript = gun.GetComponent<GunScript>();
        if (gunScript != null) {
            gunScript.recoilForce = recoil;
            gunScript.fireRate = fireRate;
            gunScript.isAutomatic = isAutomatic;
        }
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

    public void sendRayCastAndPlayHitEffect() {
        Vector2 origin = gun.transform.position;
        Vector2 direction = gun.transform.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, hitLayers);

        if (hit.collider != null) {
            Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
        }
    }
    
    public void changeToWeaponSprite1(SpriteRenderer spriteRenderer) {
        Sprite changedSprite = Resources.Load<Sprite>("Art/Weapons/Weapon1");

        if (changedSprite != null) {
            spriteRenderer.sprite = changedSprite;
        } else {
            Debug.LogError("Sprite not found at the specified path.");
        }
    }

    public void changeToWeaponSprite2(SpriteRenderer spriteRenderer) {
        Sprite changedSprite = Resources.Load<Sprite>("Art/Weapons/SimpleWeapon");

        if (changedSprite != null) {
            spriteRenderer.sprite = changedSprite;
        } else {
            Debug.LogError("Sprite not found at the specified path.");
        }
    }
}
