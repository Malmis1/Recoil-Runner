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

    private float nextFireTime;

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
            rb.velocity = firePoint.right * bulletSpeed;
        }
    }
}