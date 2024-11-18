using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Tooltip("The tag of the portal/goal")]
    public string portalTag = "Portal";

    [Tooltip("The tag of objects that take damage to the player")]
    public string damageTag = "Damage";

    [Tooltip("Reference to the win script")]
    public WinScript winScript;

    [Tooltip("Reference to the game-over script")]
    public GameOverScript gameOverScript;

    [Tooltip("Cooldown time before triggering game over after taking damage")]
    public float damageCooldown = 1f;

    private bool canTakeDamage = true;

    // Reference to the PlayerController
    private PlayerController playerController;

    [Tooltip("The audioclip for the player winning")]
    public AudioClip winSound;

    private AudioSource audioSource;


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(portalTag))
        {
            FindObjectOfType<LevelManager>()?.UnlockNextLevel();
            winScript.PauseAndShowWinMenu();

            audioSource.PlayOneShot(winSound);
        }

        if (collision.gameObject.CompareTag(damageTag) && canTakeDamage)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        if (playerController != null)
        {
            playerController.SetState(PlayerController.PlayerState.Dead);
        }

        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        gameOverScript.PauseAndShowGOMenu();
        canTakeDamage = true;
    }
}
