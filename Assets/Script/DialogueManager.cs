using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class DialogueManager : MonoBehaviour
{
    public enum CharacterEffect { None, SlideIn, Surprise, Shaking, ScreenPunch, ScaleUp, ScaleDown }

    [Header("기본 연동 컴포넌트")]
    public TypewriterEffect typewriter;
    public Image leftCharacter;
    public Image rightCharacter;
    public TMP_Text nameText;

    [Header("스텐딩 스프라이트 배열")]
    public Sprite[] leftExpressions;
    public Sprite[] rightExpressions;

    [Header("시나리오 데이터")]
    public string[] dialogues;
    public string[] characterNames;

    [Header("대사별 연출 효과 설정")]
    public CharacterEffect[] dialogueEffects;

    [Header("엔딩 및 분기 UI 연동")]
    public GameObject endingChoicePanel; // 대사가 끝난 후 켜질 선택창 UI (버튼들이 있는 부모 오브젝트)

    [Header("페이드 연출 컴포넌트")]
    public Image fadeImage; // 화면 전체를 덮는 검은색 UI Image

    [Header("ScreenPunch 대상 지정")]
    public RectTransform dialogueBoxRect; // [수정] 흔들고 싶은 진짜 대화창 UI 패널을 여기에 넣어주세요!

    private int index = 0;
    private RectTransform leftRect;
    private RectTransform rightRect;
    private Coroutine currentEffectCoroutine;

    void Start()
    {
        if (leftCharacter != null) leftRect = leftCharacter.GetComponent<RectTransform>();
        if (rightCharacter != null) rightRect = rightCharacter.GetComponent<RectTransform>();

        if (endingChoicePanel != null) endingChoicePanel.SetActive(false);

        // 시작할 때 화면이 검은색에서 서서히 밝아지는 페이드 인 효과
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeInCoroutine());
        }

        ShowDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        if (dialogues == null || dialogues.Length == 0 || index >= dialogues.Length) return;

        if (typewriter != null) typewriter.StartTyping(dialogues[index]);
        if (nameText != null && characterNames.Length > index) nameText.text = characterNames[index];

        CharacterEffect currentEffect = CharacterEffect.None;
        if (dialogueEffects != null && dialogueEffects.Length > index)
        {
            currentEffect = dialogueEffects[index];
        }

        if (characterNames[index] == "메제드")
        {
            if (leftCharacter != null)
            {
                leftCharacter.color = Color.white;
                if (leftExpressions.Length > index && leftExpressions[index] != null)
                    leftCharacter.sprite = leftExpressions[index];

                ApplyEffect(leftRect, currentEffect);
            }
            if (rightCharacter != null) rightCharacter.color = new Color(0.3f, 0.3f, 0.3f, 1f);
        }
        else
        {
            if (leftCharacter != null) leftCharacter.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            if (rightCharacter != null)
            {
                rightCharacter.color = Color.white;
                if (rightExpressions.Length > index && rightExpressions[index] != null)
                    rightCharacter.sprite = rightExpressions[index];

                ApplyEffect(rightRect, currentEffect);
            }
        }
    }

    void NextDialogue()
    {
        if (typewriter == null) return;

        if (typewriter.IsTyping())
        {
            typewriter.SkipTyping(dialogues[index]);
            return;
        }

        if (index < dialogues.Length - 1)
        {
            index++;
            ShowDialogue();
        }
        else
        {
            OnDialogueComplete();
        }
    }

    // DialogueManager.cs의 맨 아래에 있는 이 함수를 통째로 덮어쓰세요!
    void OnDialogueComplete()
    {
        // 현재 활성화된 씬의 이름을 가져옵니다.
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // 상황 1: 1번 인트로 씬일 경우 -> 자동으로 2번 심판장 씬으로 페이드 전환
        if (currentSceneName == "Scene_01_Intro")
        {
            Debug.Log("인트로 대사 종료: 자동으로 심판장 씬으로 이동합니다.");
            StartCoroutine(TransitionToSceneCoroutine("Scene_02_Ending_True"));
        }
        // 상황 2: 2번 심판장(True) 씬일 경우 -> 다음 씬으로 안 넘어가고 선택지 판넬만 활성화!
        else if (currentSceneName == "Scene_02_Ending_True")
        {
            Debug.Log("마아트 반전 대사 종료: 최종 선택지 판넬을 활성화합니다.");
            if (endingChoicePanel != null)
            {
                endingChoicePanel.SetActive(true); // 인스펙터에 연결된 판넬 켜기!
            }
        }
        // 상황 3: 3번 배드엔딩 씬일 경우 -> 일단 선택지 판넬만 활성화 (필요시 사용)
        else
        {
            if (endingChoicePanel != null) endingChoicePanel.SetActive(true);
        }
    }



    void ApplyEffect(RectTransform target, CharacterEffect effect)
    {
        if (effect == CharacterEffect.None) return;

        if (currentEffectCoroutine != null)
            StopCoroutine(currentEffectCoroutine);

        if (target != null) target.localScale = Vector3.one;

        switch (effect)
        {
            case CharacterEffect.SlideIn: if (target != null) currentEffectCoroutine = StartCoroutine(SlideInCoroutine(target)); break;
            case CharacterEffect.Surprise: if (target != null) currentEffectCoroutine = StartCoroutine(SurpriseCoroutine(target)); break;
            case CharacterEffect.Shaking: if (target != null) currentEffectCoroutine = StartCoroutine(ShakingCoroutine(target)); break;
            case CharacterEffect.ScreenPunch: if (dialogueBoxRect != null) currentEffectCoroutine = StartCoroutine(ScreenPunchCoroutine(dialogueBoxRect)); break;
            case CharacterEffect.ScaleUp: if (target != null) currentEffectCoroutine = StartCoroutine(ScaleChangeCoroutine(target, 1.2f)); break;
            case CharacterEffect.ScaleDown: if (target != null) currentEffectCoroutine = StartCoroutine(ScaleChangeCoroutine(target, 1.0f)); break;
        }
    }

    // --- [새로 추가] 버튼 클릭 시 호출할 엔딩 분기 함수들 ---

    // 1. 단서가 있는 상태로 심판장에 입장 (True Ending 씬으로 이동)
    public void SelectTrueEndingRoute(string sceneName)
    {
        StartCoroutine(TransitionToSceneCoroutine(sceneName));
    }

    // 2. 단서가 없는 상태로 심판장에 입장 (Bad Ending 씬으로 이동)
    public void SelectBadEndingRoute(string sceneName)
    {
        StartCoroutine(TransitionToSceneCoroutine(sceneName));
    }

    // --- 애니메이션 및 페이드 코루틴 모음 ---

    IEnumerator FadeInCoroutine()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 1.5f; // 페이드 속도
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator TransitionToSceneCoroutine(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 1.5f;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneName); // 어두워진 후 씬 전환
    }

    IEnumerator SlideInCoroutine(RectTransform rect)
    {
        Vector2 endPos = rect.anchoredPosition;
        Vector2 startPos = endPos + new Vector2(endPos.x > 0 ? 400f : -400f, 0f);
        rect.anchoredPosition = startPos;
        float time = 0f;
        while (time < 1f) { time += Time.deltaTime * 3f; rect.anchoredPosition = Vector2.Lerp(startPos, endPos, time); yield return null; }
        rect.anchoredPosition = endPos;
    }

    IEnumerator SurpriseCoroutine(RectTransform rect)
    {
        Vector2 originPos = rect.anchoredPosition;
        Vector2 jumpPos = originPos + new Vector2(0f, 60f);
        float time = 0f;
        while (time < 1f) { time += Time.deltaTime * 12f; rect.anchoredPosition = Vector2.Lerp(originPos, jumpPos, time); yield return null; }
        time = 0f;
        while (time < 1f) { time += Time.deltaTime * 10f; rect.anchoredPosition = Vector2.Lerp(jumpPos, originPos, time); yield return null; }
        rect.anchoredPosition = originPos;
    }

    IEnumerator ShakingCoroutine(RectTransform rect)
    {
        Vector2 originPos = rect.anchoredPosition;
        float duration = 0.5f, timer = 0f, shakeMagnitude = 10f;
        while (timer < duration) { timer += Time.deltaTime; rect.anchoredPosition = originPos + new Vector2(Random.Range(-shakeMagnitude, shakeMagnitude), 0f); yield return null; }
        rect.anchoredPosition = originPos;
    }

    IEnumerator ScreenPunchCoroutine(RectTransform rect)
    {
        Vector2 originPos = rect.anchoredPosition;
        float duration = 0.3f, timer = 0f, punchMagnitude = 15f;
        while (timer < duration) { timer += Time.deltaTime; rect.anchoredPosition = originPos + new Vector2(Random.Range(-punchMagnitude, punchMagnitude), Random.Range(-punchMagnitude, punchMagnitude)); yield return null; }
        rect.anchoredPosition = originPos;
    }

    IEnumerator ScaleChangeCoroutine(RectTransform rect, float targetScale)
    {
        Vector3 startScale = rect.localScale;
        Vector3 endScale = new Vector3(targetScale, targetScale, 1f);
        float time = 0f;
        while (time < 1f) { time += Time.deltaTime * 4f; rect.localScale = Vector3.Lerp(startScale, endScale, time); yield return null; }
        rect.localScale = endScale;
    }
}
