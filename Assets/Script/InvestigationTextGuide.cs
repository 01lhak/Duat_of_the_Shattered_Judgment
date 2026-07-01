using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class InvestigationGuideGroup : MonoBehaviour
{
    [Header("딜레이 설정 (초)")]
    [Tooltip("게임 시작 후 이 시간만큼 기다렸다가 UI가 나타납니다.")]
    [SerializeField] private float startDelay = 2.0f;

    [Header("시간 설정 (초)")]
    [Tooltip("UI가 완전히 켜진 상태로 유지되는 시간입니다.")]
    [SerializeField] private float textDuration = 3.0f;
    [Tooltip("UI가 부드럽게 투명해지며 사라지는 시간입니다.")]
    [SerializeField] private float fadeDuration = 1.5f;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // 1. 시작할 때는 UI를 완전히 투명하게 숨겨둡니다.
        canvasGroup.alpha = 0f;

        // 연출 프로세스 시작
        StartCoroutine(InvestigationGuideSequence());
    }

    private IEnumerator InvestigationGuideSequence()
    {
        // 2. 특정 타이밍(설정한 딜레이 시간)까지 대기합니다.
        yield return new WaitForSeconds(startDelay);

        // 3. UI를 완전히 보여줍니다.
        canvasGroup.alpha = 1f;

        // 4. 설정한 유지 시간 동안 화면에 띄워둡니다.
        yield return new WaitForSeconds(textDuration);

        // 5. 부드럽게 알파값을 낮춰 페이드아웃을 진행합니다.
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        // 6. 연출이 끝나면 오브젝트를 비활성화합니다.
        gameObject.SetActive(false);
    }
}
