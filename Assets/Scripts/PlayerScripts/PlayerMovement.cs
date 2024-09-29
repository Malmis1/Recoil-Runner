using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Tooltip("The movement controller for the player")]
    public PlayerController controller;

    float horizontalMove = 0f;

    [Tooltip("The movement speed")]
    public float moveSpeed = 40f;

    bool jump = false;
    
    void Update() { // Get input
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        Vector3 mouseDirecton = Input.mousePosition;

        controller.LookAtPoint(mouseDirecton);

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }
    }

    void FixedUpdate() { // FixedUpdate good for constant updating of movement. It is independent of framerate in contrast to Update
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
