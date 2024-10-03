using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Tooltip("The movement controller for the player")]
    public PlayerController controller;

    float horizontalMove = 0f;
    bool jump = false;

    void Update() { // Get input
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        if (Input.GetButton("Fire1")) { // Disable air control when firing
            controller.ReduceAirControl();
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
