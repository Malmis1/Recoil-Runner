using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public Text targetText;
    public float typingSpeed = 0.05f;

    public IEnumerator StartTypingAndWait(string newText, float absoluteStartTime, float fadeOutStartTime)
    {
        // Wait until it's time for this message to start
        float currentTime = Time.timeSinceLevelLoad;
        float delay = Mathf.Max(0, absoluteStartTime - currentTime);
        yield return new WaitForSeconds(delay);

        // Initialize text and start typing
        targetText.text = "";
        targetText.color = new Color(targetText.color.r, targetText.color.g, targetText.color.b, 1f);

        // Type the text character by character
        foreach (char c in newText)
        {
            targetText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait until it's time to start fading out
        currentTime = Time.timeSinceLevelLoad;
        float timeUntilFadeOut = fadeOutStartTime - currentTime;
        if (timeUntilFadeOut > 0)
        {
            yield return new WaitForSeconds(timeUntilFadeOut);
        }

        // Start fading out
        yield return StartCoroutine(FadeOut(1f)); // Fade out over 1 second
    }


    private IEnumerator FadeOut(float duration)
    {
        Color originalColor = targetText.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        targetText.text = "";
    }
}
