using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Tooltip("The movement controller for the player")]
    public PlayerController controller;

    private GunScript gunScript;

    float horizontalMove = 0f;
    bool jump = false;

    private void Awake()
    {
        if (gunScript == null)
        {
            gunScript = GetComponentInChildren<GunScript>();
        }
    }

    void Update() { // Get input
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) {
            jump = true;
        }

        // Detect when the player starts firing
        if (Input.GetButtonDown("Fire1")) {
            controller.AdjustAirControl(gunScript.recoilForce);
        }

        // Detect when the player stops firing
        if (Input.GetButtonUp("Fire1")) {
            controller.StartRestoreAirControl(gunScript.recoilForce);
        }
    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
