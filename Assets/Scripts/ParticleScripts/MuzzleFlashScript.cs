using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashScript : MonoBehaviour
{
    bool isMoved = false;
    public void MoveMuzzleFlash() {
        if (!isMoved) {
            transform.position += new Vector3(0, 2, 0);
            isMoved = true;
        } else {
            transform.position += new Vector3(0, -2, 0);
            isMoved = false;
        }
    }
}
