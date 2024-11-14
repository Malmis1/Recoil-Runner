using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrapScript : MonoBehaviour {
    private BoxCollider2D collider;
    private Animator animator;

    [Tooltip("The amount of time the trap will be on.")]
    public float toggleIntervalTrapOn = 2f;

    [Tooltip("The amount of time the trap will be off.")]
    public float toggleIntervalTrapOff = 4f;

    void Start() {
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(ToggleCollider());
    }

    IEnumerator ToggleCollider() {
        while (true) {
            collider.enabled = true;
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);

            yield return new WaitForSeconds(toggleIntervalTrapOn);

            collider.enabled = false;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isIdle", true);

            yield return new WaitForSeconds(toggleIntervalTrapOff);
        }
    }
}
