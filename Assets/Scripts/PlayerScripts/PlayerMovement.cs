using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The movement controller for the player")]
    public PlayerController controller;

    float horizontalMove = 0f;

    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    [Tooltip("The gun object the player character will hold")]
    public GameObject gun;

    bool jump = false;
    
    void Update() // Get input
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        LookAtPoint(Input.mousePosition);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate() // FixedUpdate good for constant updating of movement. It is independent of framerate in contrast to Update
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public void LookAtPoint(Vector3 point) { // Rotate the gun to look at the mouse
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(point) - transform.position;
        gun.transform.up = lookDirection;
    }
}
