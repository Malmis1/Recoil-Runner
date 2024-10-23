using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapons/GunData", order = 1)]
public class GunData : ScriptableObject
{
    [Header("Visuals")]
    public Sprite gunSprite;
    public GameObject muzzleFlashPrefab;

    [Tooltip("Offset for the muzzle flash position")]
    public Vector3 muzzleFlashOffset;
    
    [Header("Stats")]
    public float recoilForce = 15f;
    public float fireRate = 1.0f;
    public float additiveRecoilAngleThreshold = 250f;
    public bool initialRecoilResetsVelocity = true;
    public bool isAutomatic = false;
}
