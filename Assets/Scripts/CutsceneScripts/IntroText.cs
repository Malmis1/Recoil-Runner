using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    public TypewriterEffect typewriter;
    public Text text;
    private float sceneStartTime;
    void Start()
    {
        Time.timeScale = 1f;
        sceneStartTime = Time.timeSinceLevelLoad;
        StartCoroutine(DisplayMessages());
    }

    IEnumerator DisplayMessages()
    {
        typewriter.targetText = text;

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "A city was once a peaceful haven, protected by         legendary warriors who ensured harmony and safety for all who lived there.",
            1.4f,
            9f, sceneStartTime));

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "But everything changed when blue-skinned aliens        invaded, bringing destruction and terror that plunged the once-peaceful city into chaos.",
            10f,
            19.2f, sceneStartTime));

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "The mighty warriors, despite their strength, were      defeated, leaving the city under alien control, ruled through fear and oppression.",
            20.2f,
            30.3f, sceneStartTime));

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "On his way to work, after his car broke down, a         ordinary citizen unexpectedly stumbled upon a            mysterious and glowing artifact.",
            31.5f,
            41f, sceneStartTime));

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "The artifact gave him the power to wield the warriors' legendary weapons, but as an ordinary person, he      struggles to control their immense knockback.",
            42f,
            53f, sceneStartTime));

        yield return StartCoroutine(typewriter.StartTypingAndWait(
            "Realizing that this newfound power might be the city’s last hope, he resolves to rise to the challenge and      fight back against the alien invaders.",
            54f,
            63f, sceneStartTime));
    }
}
