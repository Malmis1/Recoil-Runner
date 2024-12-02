using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Weapons/GunData", order = 1)]
public class GunData : ScriptableObject
{
    [Header("Visuals")]
    public Sprite gunSprite;
    public Sprite hudSprite;
    [Space(10)]
    public GameObject muzzleFlashPrefab;
    [Tooltip("Offset for the muzzle flash position")]
    public Vector3 muzzleFlashOffset;
    [Space(10)]
    public GameObject bulletTrailPrefab;
    [Tooltip("Duration the bullet trail will be visible before fading out")]
    public float bulletTrailFadeDuration = 0.3f;
    [Space(10)]
    [Tooltip("Particle effect to play at the hit point")]
    public GameObject hitEffectPrefab;
    [Space(10)]
    [Tooltip("The audio clip for shooting")]
    public AudioClip shootingAudio;

    [Header("Stats")]
    public float recoilForce = 15f;
    public int ammo = 30;
    public float fireRate = 1.0f;
    public int shotsPerFire = 1;
    public float spreadAngle = 1f;
    public float additiveRecoilAngleThreshold = 250f;
    public bool initialRecoilResetsVelocity = true;
    public bool isAutomatic = false;
    public float damage = 20.0f;
    public bool hasDoubleShot = false;
}
