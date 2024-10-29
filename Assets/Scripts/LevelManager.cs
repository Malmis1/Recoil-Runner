using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Minimum X value for camera position in this level.")]
    [SerializeField] private float minX;

    [Tooltip("Maximum X value for camera position in this level.")]
    [SerializeField] private float maxX;

    [Header("Player Settings")]
    [Tooltip("Should the player start with a gun?")]
    [SerializeField] public bool startWithGun;

    [Tooltip("The index of the gun to start with if 'Start With Gun' is enabled.")]
    [SerializeField] public int gunIndex;

    private CameraFollow cameraFollow;
    private WeaponController weaponController;

    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.MinX = minX;
            cameraFollow.MaxX = maxX;

        }
        else
        {
            Debug.LogWarning("CameraFollow component not found on the main camera.");
        }

        GameObject player = GameObject.FindWithTag("Player"); 
        if (player != null)
        {
            weaponController = player.GetComponent<WeaponController>();
            if (weaponController != null)
            {
                if (startWithGun)
                {
                    weaponController.ChangeGun(gunIndex); 
                }
                else
                {
                    weaponController.DeactivateGun(); 
                }
            }
            else
            {
                Debug.LogWarning("WeaponController component not found on the player.");
            }
        }
        else
        {
            Debug.LogWarning("Player GameObject with tag 'Player' not found.");
        }
    }
}
