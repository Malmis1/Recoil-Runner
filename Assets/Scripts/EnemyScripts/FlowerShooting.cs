using System.Collections;
using UnityEngine;

public class FlowerShooting : MonoBehaviour {
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

    private float nextFireTime;

    void Update() {
        if (Time.time >= nextFireTime) {
            controller.EnemyAttack();
            StartCoroutine(DelayShoot(0.3f));

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

        StartCoroutine(WaitAndSetIdle(0.7f));
    }

    private IEnumerator DelayShoot(float duration) {
        yield return new WaitForSeconds(duration);
        Shoot();
    }

    private IEnumerator WaitAndSetIdle(float duration) {
        yield return new WaitForSeconds(duration);
        controller.SetState(EnemyController.EnemyState.Idle);
    }
}
