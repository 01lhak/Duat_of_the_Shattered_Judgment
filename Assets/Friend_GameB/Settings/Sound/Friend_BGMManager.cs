using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class Friend_BGMManager : MonoBehaviour
{
    public static Friend_BGMManager Instance;

    [Header("배경음 오디오 소스")]
    public AudioSource mainBGM;          // 그룹 A : 메뉴, 저장창, 설정창 공유
    public AudioSource storyBGM;         // 그룹 B : 스토리 1 전용
    public AudioSource investigationBGM; // 그룹 C : 모든 조사/탐험 맵 공유

    [Header("페이드아웃 설정 (초)")]
    [Tooltip("스토리 씬 진입 등으로 음악이 부드럽게 꺼지는 시간입니다.")]
    [SerializeField] private float fadeOutDuration = 2.0f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        HandleSceneBGM(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleSceneBGM(scene.name);
    }

    /// <summary>
    /// 💥 씬 이름에 맞춰 완벽하게 분기 처리하는 핵심 함수
    /// </summary>
    private void HandleSceneBGM(string sceneName)
    {
        // ----------------------------------------------------
        // [그룹 A] 메인메뉴, 세이브로드, 옵션창 (노래 공유 및 유지)
        // ----------------------------------------------------
        if (sceneName == "1Mainmenu" || sceneName == "3Saveload" || sceneName == "4Options")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            PlayMainBGM();
        }
        // ----------------------------------------------------
        // [그룹 B] 스토리 1 씬 (단독 브금)
        // ----------------------------------------------------
        else if (sceneName == "5Story1")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            PlayStoryBGM();
        }
        // ----------------------------------------------------
        // [그룹 C] 모든 조사맵 (노래 공유 및 끊김 없는 루프 연동)
        // ----------------------------------------------------
        else if (sceneName == "6.1" || sceneName == "71" || sceneName == "8.3" ||
                 sceneName == "9.4_1" || sceneName == "9.4_2" || sceneName == "10.5_1" || sceneName == "10.5_2")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            PlayInvestigationBGM();
        }
        // ----------------------------------------------------
        // [영상 예외] 로딩 영상 씬 (자체 사운드가 있으므로 매니저 브금 전원 오프)
        // ----------------------------------------------------
        else if (sceneName == "2GameLoading")
        {
            StopAll();
        }
        // ----------------------------------------------------
        // [스토리 연출 예외] 이전 질문의 스토리 인트로 진입 시 부드럽게 끄기
        // ----------------------------------------------------
        else if (sceneName == "Scene_01_Intro")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeOutAllBGM(fadeOutDuration));
        }
    }

    void PlayMainBGM()
    {
        if (mainBGM == null) return;
        if (mainBGM.isPlaying) return; // ⭐ 이미 메뉴 음악이 나오고 있다면 끊지 않고 보존!

        storyBGM?.Stop();
        investigationBGM?.Stop();

        mainBGM.loop = true;
        mainBGM.volume = 1f;
        mainBGM.Play();
    }

    void PlayStoryBGM()
    {
        if (storyBGM == null) return;
        if (storyBGM.isPlaying) return;

        mainBGM?.Stop();
        investigationBGM?.Stop();

        storyBGM.loop = true;
        storyBGM.volume = 1f;
        storyBGM.Play();
    }

    void PlayInvestigationBGM()
    {
        if (investigationBGM == null) return;
        if (investigationBGM.isPlaying) return; // ⭐ 6.1에서 71 등으로 가도 재생 중이면 툭 끊기지 않음!

        mainBGM?.Stop();
        storyBGM?.Stop();

        investigationBGM.loop = true;
        investigationBGM.volume = 1f;
        investigationBGM.Play();
    }

    private IEnumerator FadeOutAllBGM(float duration)
    {
        AudioSource activeSource = null;
        if (mainBGM != null && mainBGM.isPlaying) activeSource = mainBGM;
        if (storyBGM != null && storyBGM.isPlaying) activeSource = storyBGM;
        if (investigationBGM != null && investigationBGM.isPlaying) activeSource = investigationBGM;

        if (activeSource == null) yield break;

        float startVolume = activeSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            activeSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
            yield return null;
        }

        activeSource.Stop();
        activeSource.volume = startVolume;
    }

    public void StopAll()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (mainBGM != null) mainBGM.Stop();
        if (storyBGM != null) storyBGM.Stop();
        if (investigationBGM != null) investigationBGM.Stop();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
