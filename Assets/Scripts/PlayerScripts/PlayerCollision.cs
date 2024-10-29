using System.Collections;
using System.Collections.Generic;
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
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(portalTag)) {
            winScript.PauseAndShowWinMenu();
        }

        if (collision.gameObject.CompareTag(damageTag)) {
            Debug.Log("Player took damage by: " + collision.gameObject.name);

            gameOverScript.PauseAndShowGOMenu();
        }
    }
}
