using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killPlane : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCollision playerCollision = other.GetComponent<PlayerCollision>();
        if (playerCollision != null)
        {
            playerCollision.KillPlayer();
        }
        
    }
}
