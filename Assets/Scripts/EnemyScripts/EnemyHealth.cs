using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Tooltip("The controller for the enemy")]
    public EnemyController controller;

    [Tooltip("The health of the enemy")]
    public float health = 1.0f;

    [Tooltip("Color to flash when taking damage")]
    public Color damageColor = Color.red;

    [Tooltip("Time to flash red when damaged")]
    public float flashDuration = 0.2f;

    private bool isDead = false;
    private bool initialized = false;
    private float initDelay = 0.1f;
    private SpriteRenderer spriteRenderer;
    private Coroutine flashCoroutine;
    private Color defaultColor;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the enemy!");
        }
        else
        {
            defaultColor = spriteRenderer.color;
        }

        // Give time for physics to initialize
        Invoke("SetInitialized", initDelay);
    }

    void SetInitialized()
    {
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        if (health <= 0f && !isDead)
        {
            isDead = true;
            GameManager.Instance.IncrementKillCount();
            controller.EnemyDie();
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isDead && initialized)
        {
            health -= amount;

            // Trigger red flash
            if (spriteRenderer != null)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                }
                flashCoroutine = StartCoroutine(FlashRed());
            }
        }
    }

    public void AddHealth(float amount)
    {
        health += amount;
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = defaultColor;
    }
}
