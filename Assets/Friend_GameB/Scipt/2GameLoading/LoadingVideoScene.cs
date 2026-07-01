using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class LoadingVideoScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    [Header("원신 스타일 스킵 UI")]
    public RectTransform skipButtonRect; // 버튼의 위치 제어용 (RectTransform)
    public CanvasGroup skipButtonAlpha;  // 버튼의 투명도 제어용 (CanvasGroup)

    [Header("연출 세부 설정")]
    public float autoHideDelay = 3.0f;   // 버튼 유지 시간 (3초)
    public float fadeDuration = 0.3f;    // 나타나고 사라지는 시간 (0.3초)
    public float moveOffset = 50f;       // 위에서 아래로 내려올 이동 거리 (50픽셀)

    private Vector2 anchoredPositionTarget; // 버튼의 원래 정석 위치
    private Vector2 anchoredPositionStart;  // 버튼이 숨어있을 위쪽 위치

    private Coroutine stateCoroutine;
    private bool isShown = false;
    private bool isTransitioning = false; // 씬 전환 중 중복 클릭 방지

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += LoadNextScene;
            videoPlayer.Play();
        }

        if (skipButtonRect != null && skipButtonAlpha != null)
        {
            // 원래 저장된 우측 상단 정석 위치를 기억합니다.
            anchoredPositionTarget = skipButtonRect.anchoredPosition;
            // 시작 위치는 원래 위치보다 moveOffset만큼 위쪽(Y축 +) 공간입니다.
            anchoredPositionStart = anchoredPositionTarget + new Vector2(0, moveOffset);

            // 초기 상태: 완전히 투명하게 하고 위쪽에 숨겨둡니다.
            skipButtonAlpha.alpha = 0f;
            skipButtonAlpha.blocksRaycasts = false;
            skipButtonRect.anchoredPosition = anchoredPositionStart;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 이미 씬이 넘어가고 있는 중이라면 화면 클릭을 무시합니다.
            if (isTransitioning) return;

            // 화면을 터치하면 연출을 새로 시작합니다.
            ShowSkipButton();
        }
    }

    private void ShowSkipButton()
    {
        if (skipButtonRect == null || skipButtonAlpha == null) return;

        // 기존에 작동 중이던 타이머나 코루틴이 있다면 강제로 정지시킵니다.
        if (stateCoroutine != null)
        {
            StopCoroutine(stateCoroutine);
        }

        // 나타나기 연출 + 3초 대기 + 사라지기 연출을 하나의 흐름으로 실행합니다.
        stateCoroutine = StartCoroutine(ButtonSequenceRoutine());
    }

    // 부드러운 [나타나기 -> 대기 -> 사라지기] 전체 프로세스 코루틴
    private IEnumerator ButtonSequenceRoutine()
    {
        isShown = true;
        skipButtonAlpha.blocksRaycasts = true; // 버튼 클릭 가능하게 켜기

        // 1. 위에서 아래로 스륵 내려오며 선명해지기 (Fade In & Move Down)
        float timer = 0f;
        Vector2 currentPos = skipButtonRect.anchoredPosition;
        float currentAlpha = skipButtonAlpha.alpha;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            skipButtonAlpha.alpha = Mathf.Lerp(currentAlpha, 1f, t);
            skipButtonRect.anchoredPosition = Vector2.Lerp(currentPos, anchoredPositionTarget, t);
            yield return null;
        }

        skipButtonAlpha.alpha = 1f;
        skipButtonRect.anchoredPosition = anchoredPositionTarget;

        // 2. 원신처럼 지정된 시간(3초) 동안 화면에 머무르며 버티기
        yield return new WaitForSeconds(autoHideDelay);

        // 3. 다시 위쪽으로 스륵 올라가며 투명해지기 (Fade Out & Move Up)
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            skipButtonAlpha.alpha = Mathf.Lerp(1f, 0f, t);
            skipButtonRect.anchoredPosition = Vector2.Lerp(anchoredPositionTarget, anchoredPositionStart, t);
            yield return null;
        }

        skipButtonAlpha.alpha = 0f;
        skipButtonAlpha.blocksRaycasts = false; // 숨었을 때는 클릭 안 되게 막기
        skipButtonRect.anchoredPosition = anchoredPositionStart;
        isShown = false;
    }

    // 우측 상단 [건너뛰기] 버튼을 누르면 터질 함수
    public void OnClickSkipButton()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (stateCoroutine != null) StopCoroutine(stateCoroutine);

        // 버튼을 누르면 즉시 위로 스륵 사라지는 연출만 따로 실행하면서 동시에 씬을 넘깁니다.
        StartCoroutine(ExitButtonAnimationRoutine());
        LoadNextScene(videoPlayer);
    }

    // 버튼 클릭 시 기존 위치에서 위로 스륵 올라가며 사라지는 연출
    private IEnumerator ExitButtonAnimationRoutine()
    {
        skipButtonAlpha.blocksRaycasts = false;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            skipButtonAlpha.alpha = Mathf.Lerp(1f, 0f, t);
            skipButtonRect.anchoredPosition = Vector2.Lerp(anchoredPositionTarget, anchoredPositionStart, t);
            yield return null;
        }
    }

    private void LoadNextScene(VideoPlayer vp)
    {
        if (vp != null)
        {
            vp.loopPointReached -= LoadNextScene;
        }
        SceneManager.LoadScene(4);
    }
}
