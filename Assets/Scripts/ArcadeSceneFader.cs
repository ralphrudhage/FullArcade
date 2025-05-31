using System.Collections;
using UnityEngine;

public class ArcadeSceneFader : MonoBehaviour
{
    private CanvasGroup cg;

    private void OnEnable()
    {
        cg = GetComponent<CanvasGroup>();
    }
    
    public void FadeIn()
    {
        StartCoroutine(FadeAlpha());
    }

    public void BlackScene()
    {
        cg.alpha = 1;
    }

    private IEnumerator FadeAlpha(float duration = 2f, float startAlpha = 1f, float endAlpha = 0f)
    {
        // Set initial alpha
        cg.alpha = startAlpha;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the new alpha value
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            yield return null;
        }

        // Ensure the final alpha value is set
        cg.alpha = endAlpha;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAlpha(2f, 0f, 1f));
    }
}