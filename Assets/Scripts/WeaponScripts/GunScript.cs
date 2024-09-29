using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour {
    [Tooltip("The movement controller for the weapon")]
    public WeaponController controller;

    void Update() {
        Vector3 mouseDirecton = Input.mousePosition;

        controller.LookAtPoint(mouseDirecton);

    }
}
