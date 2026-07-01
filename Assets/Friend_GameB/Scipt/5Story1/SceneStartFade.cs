using UnityEngine;
using UnityEngine.UI;
using System;

public class SceneStartFade : MonoBehaviour
{
    public Image fadeImage;
    public static bool isFading;

    public Action onFadeComplete;

    void Start()
    {
        StartCoroutine(Fade());
    }

    System.Collections.IEnumerator Fade()
    {
        isFading = true;

        Color c = fadeImage.color;

        float t = 0;
        while (t < 2f)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / 2f);
            fadeImage.color = c;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);

        isFading = false;
        onFadeComplete?.Invoke();
    }
}