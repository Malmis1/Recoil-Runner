using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour {
    [Tooltip("The movement controller for the weapon")]
    public WeaponController controller;

    void Update() {
        Vector3 mouseDirecton = Input.mousePosition;

        controller.LookAtPoint(mouseDirecton);

        if (Input.GetButtonDown("Fire1")) {
            controller.ApplyRecoil();
        }
    }
}
