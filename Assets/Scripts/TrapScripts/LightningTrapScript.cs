using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTrapScript : MonoBehaviour {
    private BoxCollider2D trapCollider;
    private Animator animator;

    [Tooltip("The amount of time the trap will be on.")]
    public float toggleIntervalTrapOn = 2f;

    [Tooltip("The amount of time the trap will be off.")]
    public float toggleIntervalTrapOff = 4f;

    [Tooltip("The audioclip for the trap attacking")]
    public AudioClip lightningSound;

    private AudioSource audioSource;

    void Start() {
        trapCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(ToggleCollider());
    }

    IEnumerator ToggleCollider() {
        while (true) {
            trapCollider.enabled = true;
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);

            audioSource.PlayOneShot(lightningSound);

            yield return new WaitForSeconds(toggleIntervalTrapOn);

            trapCollider.enabled = false;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isIdle", true);

            audioSource.Stop();

            yield return new WaitForSeconds(toggleIntervalTrapOff);
        }
    }
}
