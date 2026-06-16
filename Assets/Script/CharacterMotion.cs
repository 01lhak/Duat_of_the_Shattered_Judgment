using UnityEngine;
using System.Collections;

public class CharacterMotion : MonoBehaviour
{
    public RectTransform rectTransform;

    public void SlideIn(Vector2 start, Vector2 end)
    {
        StartCoroutine(Slide(start, end));
    }

    IEnumerator Slide(Vector2 start, Vector2 end)
    {
        rectTransform.anchoredPosition = start;

        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * 2f;
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, time);
            yield return null;
        }
    }
}