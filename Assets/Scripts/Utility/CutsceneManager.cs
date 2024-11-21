using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public RectTransform[] images;
    public float fadeDuration = 1f;
    public float moveDuration = 5f;
    public Vector2 moveStartOffset = new Vector2(500, 0);
    public Vector2 moveEndOffset = new Vector2(-500, 0);

    public Camera mainCamera;
    public float zoomStart = 60f;
    public float zoomEnd = 50f;
    void Start()
    {
        foreach (RectTransform image in images)
        {
            image.gameObject.SetActive(false);
        }

        mainCamera.fieldOfView = zoomStart;

        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        foreach (RectTransform image in images)
        {
            image.gameObject.SetActive(true);

            CanvasGroup cg = image.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = image.gameObject.AddComponent<CanvasGroup>();
            }
            cg.alpha = 0;

            Vector2 originalPosition = image.anchoredPosition;
            image.anchoredPosition = originalPosition + moveStartOffset;

            yield return StartCoroutine(FadeCanvasGroup(cg, 0f, 1f, fadeDuration));

            yield return StartCoroutine(MoveImageWithZoom(image, originalPosition + moveStartOffset, originalPosition + moveEndOffset, moveDuration));

            yield return StartCoroutine(FadeCanvasGroup(cg, 1f, 0f, fadeDuration));

            image.anchoredPosition = originalPosition;

            image.gameObject.SetActive(false);
        }

        SceneManager.LoadScene("Tutorial");
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }

    private IEnumerator MoveImageWithZoom(RectTransform image, Vector2 start, Vector2 end, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            image.anchoredPosition = Vector2.Lerp(start, end, timer / duration);

            mainCamera.fieldOfView = Mathf.Lerp(zoomStart, zoomEnd, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }

        image.anchoredPosition = end;

        mainCamera.fieldOfView = zoomEnd;
    }
}
