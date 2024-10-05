using UnityEngine;

public class ShowCanvasWithFade : MonoBehaviour
{
    [Tooltip("The player character to check distance from.")]
    [SerializeField] private Transform player;  // Reference to the player's transform.
    
    [Tooltip("The CanvasGroup component that controls the fade effect.")]
    [SerializeField] private CanvasGroup canvasGroup;  // Reference to the CanvasGroup for fading.

    [Tooltip("The distance at which the canvas will start to fade in.")]
    [SerializeField] private float triggerDistance = 5f;  // Distance at which the canvas starts to fade in.
    
    [Tooltip("The speed at which the canvas fades in and out.")]
    [SerializeField] private float fadeSpeed = 2f;  // Speed of fade effect.

    private bool isFadingIn = false;  // Check if the canvas is currently fading in.
    private bool isFadingOut = false; // Check if the canvas is currently fading out.

    void Start()
    {
        // Ensure the canvas is initially invisible.
        canvasGroup.alpha = 0f;
    }

    void Update()
    {
        // Calculate the distance between the player and this object.
        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within the trigger distance, fade in.
        if (distance <= triggerDistance && canvasGroup.alpha < 1f)
        {
            FadeIn();
        }
        // If the player is further than the trigger distance, fade out.
        else if (distance > triggerDistance && canvasGroup.alpha > 0f)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        // Lerp the canvas alpha value toward 1 (fully visible).
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
    }

    void FadeOut()
    {
        // Lerp the canvas alpha value toward 0 (fully invisible).
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
    }
}
