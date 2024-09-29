using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    [Tooltip("The force/recoil which should be applied to player when weapon is fired")]
    public float recoilForce = 20f;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LookAtPoint(Vector3 point) { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }

    public void ApplyRecoil() {
        Vector2 recoilDirection = -gun.transform.up;

        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
    }
}
