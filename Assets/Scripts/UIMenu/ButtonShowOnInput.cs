using UnityEngine;
using UnityEngine.UI;

public class ButtonShowOnInput : MonoBehaviour
{
    [SerializeField] private Button button; // The button to show/hide
    [SerializeField] private float fadeDuration = 1f; // Duration for fading in/out
    [SerializeField] private float inactivityTime = 3f; // Time before fading out due to inactivity

    private CanvasGroup canvasGroup;
    private float lastInputTime; // Tracks the time of the last user input
    private bool isFadingIn;
    private bool isFadingOut;

    private void Awake()
    {
        // Add a CanvasGroup to the button if it doesn't already exist
        canvasGroup = button.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }

        // Initially hide the button and disable interaction
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Initialize the last input time to prevent the button from appearing on start
        lastInputTime = Time.time; // Start with inactivity detection
    }

    private void Update()
    {
        // Detect user input (keyboard or mouse)
        if (Input.anyKeyDown || Input.mousePresent && Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            lastInputTime = Time.time; // Reset inactivity timer

            // Fade in the button if it's not already visible
            if (!isFadingIn && canvasGroup.alpha == 0f)
            {
                StopAllCoroutines();
                StartCoroutine(FadeIn());
            }
        }
        else if (Time.time - lastInputTime > inactivityTime && !isFadingOut && canvasGroup.alpha == 1f)
        {
            // Fade out the button after the inactivity timeout
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
        float elapsedTime = 0f;

        // Gradually fade in
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        // Fully enable interaction once the fade-in is complete
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        isFadingIn = false;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;
        float elapsedTime = 0f;

        // Gradually fade out
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Fully disable interaction once the fade-out is complete
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isFadingOut = false;
    }
}
