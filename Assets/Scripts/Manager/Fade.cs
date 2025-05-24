using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage; // UI Image на весь экран, черного цвета, с альфой 0..1

    private void Awake()
    {
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(true);
    }

    public IEnumerator FadeOut(float duration)
    {
        if (fadeImage == null) yield break;

        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        if (fadeImage == null) yield break;

        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
    }
}
