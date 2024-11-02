using UnityEngine;
using UnityEngine;
using System.Collections.Generic;

public class Laser : MonoBehaviour {
    public float laserLength = 10f;

    private LineRenderer lineRenderer;
    private BoxCollider2D laserCollider;

    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        // Set up the BoxCollider2D and configure it as a trigger
        laserCollider = GetComponent<BoxCollider2D>();
        laserCollider.isTrigger = true;
        laserCollider.enabled = false; // Disable initially
    }

    public void EnableLaser() {
        lineRenderer.enabled = true;
        laserCollider.enabled = true;
        ShootLaser();
    }

    public void DisableLaser() {
        lineRenderer.enabled = false;
        laserCollider.enabled = false;
    }

    void ShootLaser() {
        Vector3 laserStart = transform.position;
        Vector3 laserEnd = transform.position - Vector3.up * laserLength;
        
        lineRenderer.SetPosition(0, laserStart);
        lineRenderer.SetPosition(1, laserEnd);

        laserCollider.offset = new Vector2(0, -laserLength / 2);
        laserCollider.size = new Vector2(0.1f, laserLength);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player hit by laser!");
        }
    }
}
