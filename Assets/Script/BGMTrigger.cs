using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    [Header("이 씬이 시작될 때 재생할 새 배경음악 파일")]
    public AudioClip newBGM;

    [Header("음악이 바뀌는 페이드 시간 (초)")]
    public float fadeTime = 0.5f;

    void Start()
    {
        // DontDestroyOnLoad로 살아남은 BGMManager를 찾아 음악을 교체 지시합니다.
        if (BGMManager.Instance != null && newBGM != null)
        {
            BGMManager.Instance.ChangeBGM(newBGM, fadeTime);
        }
    }
}
