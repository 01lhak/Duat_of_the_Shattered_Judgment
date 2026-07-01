using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class MapNameGuide : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TMP_Text mapNameText; // 맵 이름 텍스트 컴포넌트

    [Header("딜레이 설정 (초)")]
    [Tooltip("이 UI 오브젝트가 활성화된 후, 몇 초 뒤에 연출을 시작할지 설정합니다.")]
    [SerializeField] private float startDelay = 2.0f;

    [Header("시간 설정 (초)")]
    [Tooltip("부드럽게 나타나는(스르륵 켜지는) 시간입니다.")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [Tooltip("완전히 켜져서 유지되는 시간입니다.")]
    [SerializeField] private float showDuration = 2.5f;
    [Tooltip("부드럽게 사라지는(스르륵 꺼지는) 시간입니다.")]
    [SerializeField] private float fadeOutDuration = 1.0f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // 켜지는 순간에는 완전히 투명하게 초기화
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(MapNameSequence());
    }

    /// <summary>
    /// 외부에서 코드 한 줄로 직접 맵 이름을 바꾸고 연출을 키고 싶을 때 사용하는 함수
    /// </summary>
    public void ShowMapName(string newMapName)
    {
        if (this == null || mapNameText == null) return;

        mapNameText.text = newMapName;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        else
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(MapNameSequence());
        }
    }

    private IEnumerator MapNameSequence()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0f;

        // 1. 설정한 startDelay만큼 대기
        yield return new WaitForSeconds(startDelay);
        if (this == null || canvasGroup == null) yield break;

        // 2. [추가] 부드럽게 스르륵 나타나기 (Fade In)
        float fadeInTime = 0f;
        while (fadeInTime < fadeInDuration)
        {
            if (this == null || canvasGroup == null) yield break;

            fadeInTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, fadeInTime / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 3. 완전히 노출된 상태로 유지 시간만큼 대기
        yield return new WaitForSeconds(showDuration);
        if (this == null || canvasGroup == null) yield break;

        // 4. 부드럽게 스르륵 사라지기 (Fade Out)
        float fadeOutTime = 0f;
        while (fadeOutTime < fadeOutDuration)
        {
            if (this == null || canvasGroup == null) yield break;

            fadeOutTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeOutTime / fadeOutDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        // 5. 다음 연출 재사용을 위해 오브젝트 자동 비활성화
        gameObject.SetActive(false);
    }
}
