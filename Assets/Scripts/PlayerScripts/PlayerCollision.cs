using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Tooltip("The tag of the portal/goal")]
    public string specificTag = "Portal";
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(specificTag)) {
            Debug.Log("Player triggered by: " + collision.gameObject.name);
        }
    }
}
