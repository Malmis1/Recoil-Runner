using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
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

    void Update()
    { // Get input
        if (controller.state == PlayerController.PlayerState.Dead)
            return;

        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void FixedUpdate()
    {
        if (controller.state == PlayerController.PlayerState.Dead)
            return;

        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}
