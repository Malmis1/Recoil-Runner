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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(portalTag)) {
            winScript.PauseAndShowWinMenu();
        }

        if (collision.gameObject.CompareTag(damageTag) && canTakeDamage) {
            Debug.Log("Player took damage by: " + collision.gameObject.name);
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        gameOverScript.PauseAndShowGOMenu();
        canTakeDamage = true;
    }
}
