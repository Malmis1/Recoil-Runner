using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {
    [Tooltip("Bullet projectile to be spawned")]
    public GameObject bulletPrefab;

    [Tooltip("Position from where the bullet is fired")]
    public Transform firePoint;

    [Tooltip("Bullet speed")]
    public float bulletSpeed = 10f;

    [Tooltip("Time between each shot")]
    public float fireRate = 1.0f;

    [Tooltip("The enemy controller")]
    public EnemyController controller;

    [Tooltip("The audioclip for the enemy shooting")]
    public AudioClip shootSound;

    private AudioSource audioSource;

    private float nextFireTime;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (Time.time >= nextFireTime) {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null) {
            float adjustedBulletSpeed = controller.AdjustBulletSpeed(bulletSpeed);
            rb.velocity = firePoint.right * adjustedBulletSpeed;
        }

        audioSource.PlayOneShot(shootSound);
    }
}
