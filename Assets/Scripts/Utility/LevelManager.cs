using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("HUD Elements")]
    [Tooltip("The main HUD Canvas containing all HUD elements")]
    public GameObject HUDCanvas;

    [Header("Camera Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Level Settings")]
    [Tooltip("The current level number.")]
    public int currentLevel = 1;

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
            cameraFollow.MinY = minY;
            cameraFollow.MaxY = maxY;
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
                AssignHUDElements();
                if (startWithGun)
                {
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

    public void UnlockNextLevel()
    {
        int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 1);

        if (currentLevel >= levelsUnlocked)
        {
            PlayerPrefs.SetInt("LevelsUnlocked", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    private void AssignHUDElements()
    {
        if (HUDCanvas != null)
        {
            // Find AmmoCounter
            GameObject ammoCounter = HUDCanvas.transform.Find("AmmoCounter")?.gameObject;
            if (ammoCounter != null)
            {
                weaponController.ammoCounter = ammoCounter;
            }
            else
            {
                Debug.LogWarning("AmmoCounter not found in HUD.");
            }


            // Find CurrentAmmoText
            TMP_Text currentAmmoText = HUDCanvas.transform.Find("AmmoCounter/AmmoTexts/CurrentAmmoText")?.GetComponent<TMP_Text>();
            if (currentAmmoText != null)
            {

                // Check if weaponController and gunScript are assigned correctly
                if (weaponController != null && weaponController.gun != null)
                {
                    // Get GunScript from the gun GameObject
                    GunScript gunScript = weaponController.gun.GetComponent<GunScript>();
                    if (gunScript != null)
                    {
                        gunScript.currentAmmoText = currentAmmoText;
                    }
                    else
                    {
                        Debug.LogWarning("GunScript component not found on the gun GameObject.");
                    }
                }
                else
                {
                    Debug.LogWarning("weaponController or weaponController.gun is null. Ensure they are assigned correctly.");
                }
            }
            else
            {
                Debug.LogWarning("CurrentAmmoText not found or TMP_Text component missing.");
            }

            // Find GunImage
            Image gunImage = HUDCanvas.transform.Find("AmmoCounter/GunImage")?.GetComponent<Image>();
            if (gunImage != null)
            {
                if (weaponController != null)
                {
                    weaponController.hudImage = gunImage;
                }
            }
            else
            {
                Debug.LogWarning("GunImage not found in HUD.");
            }
        }
        else
        {
            Debug.LogWarning("HUDCanvas is not assigned in LevelManager.");
        }
    }

}
