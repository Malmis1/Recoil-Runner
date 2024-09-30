using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPrefab : MonoBehaviour {
    [Tooltip("Time after object is destroyed in seconds")]
    public float lifetime = 1f;

    void Start() {
        Destroy(gameObject, lifetime);
    }
}

