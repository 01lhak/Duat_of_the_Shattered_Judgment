using UnityEngine;
using UnityEngine.SceneManagement;

public class Friend_BGMManager : MonoBehaviour
{
    public static Friend_BGMManager Instance;

    [Header("배경음")]
    public AudioSource mainBGM;          // 메인 메뉴, 불러오기, 설정
    public AudioSource storyBGM;         // 스토리
    public AudioSource investigationBGM; // 조사맵

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMainBGM();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int index = scene.buildIndex;

        // 신1 : 로딩 영상
        if (index == 1)
        {
            StopAll();
        }

        // 신0 메인 / 신2 불러오기 / 신3 설정
        else if (index == 0 || index == 2 || index == 3)
        {
            PlayMainBGM();
        }

        // 신4 : 스토리
        else if (index == 4)
        {
            PlayStoryBGM();
        }

        // 신5 : 조사맵
        else if (index == 5)
        {
            PlayInvestigationBGM();
        }
    }

    void PlayMainBGM()
    {
        if (mainBGM == null) return;

        if (mainBGM.isPlaying)
            return;

        storyBGM?.Stop();
        investigationBGM?.Stop();

        mainBGM.loop = true;
        mainBGM.Play();
    }

    void PlayStoryBGM()
    {
        if (storyBGM == null) return;

        StopAll();

        storyBGM.loop = true;
        storyBGM.Play();
    }

    void PlayInvestigationBGM()
    {
        if (investigationBGM == null) return;

        StopAll();

        investigationBGM.loop = true;
        investigationBGM.Play();
    }

    void StopAll()
    {
        if (mainBGM != null)
            mainBGM.Stop();

        if (storyBGM != null)
            storyBGM.Stop();

        if (investigationBGM != null)
            investigationBGM.Stop();
    }

    public void StopBGM()
    {
        StopAll();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}