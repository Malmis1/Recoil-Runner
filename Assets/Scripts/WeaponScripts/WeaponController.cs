using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    public void LookAtPoint(Vector3 point) { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }
}
