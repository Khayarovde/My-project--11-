using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float waitTime = 1f; // <--- Добавлено

    void Start()
    {
        canvasGroup.alpha = 0;
    }

    public IEnumerator FadeToBlack()
    {
        yield return StartCoroutine(Fade(1));
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator FadeFromBlack()
    {
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
