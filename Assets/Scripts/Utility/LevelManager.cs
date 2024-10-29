using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [Header("Player Settings")]
    [SerializeField] public bool startWithGun;

    [Tooltip("The name of the gun to start with if 'Start With Gun' is enabled.")]
    [HideInInspector] public string gunName; 

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
                    Debug.Log("Starting gun: " + gunName);
                    weaponController.ChangeGunByName(gunName); 
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
