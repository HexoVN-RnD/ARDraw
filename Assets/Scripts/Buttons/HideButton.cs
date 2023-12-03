using System.Collections;
using UnityEngine;

public class HideButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup[] canvasGroupsToHide = null;
    [SerializeField]
    private float fadeDuration = 0.25f;
    private bool isFading = false;

    public void OnHideButtonClick()
    {
        if (isFading) return;

        if (canvasGroupsToHide.Length > 0 && canvasGroupsToHide[0].alpha > 0)
        {
            foreach (var canvasGroup in canvasGroupsToHide)
            {
                StartCoroutine(FadeOut(canvasGroup, fadeDuration));
            }
        }
        else
        {
            foreach (var canvasGroup in canvasGroupsToHide)
            {
                StartCoroutine(FadeIn(canvasGroup, fadeDuration));
            }
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        isFading = true;
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, t);
            yield return null;
        }

        canvasGroup.alpha = 0;
        isFading = false;
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        isFading = true;
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, t);
            yield return null;
        }

        canvasGroup.alpha = 1;
        isFading = false;
    }
}
