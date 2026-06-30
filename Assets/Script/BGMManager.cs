using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    private AudioSource audioSource;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // ⭐ [새로 추가] 새로운 배경음악으로 부드럽게 교체하는 함수
    public void ChangeBGM(AudioClip newClip, float fadeDuration = 0.5f)
    {
        if (audioSource == null) return;

        // 이미 같은 음악이 재생 중이라면 아무것도 하지 않음
        if (audioSource.clip == newClip && audioSource.isPlaying) return;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeAndChangeClip(newClip, fadeDuration));
    }

    // 음악을 자연스럽게 줄였다가 새 음악을 켜는 코루틴
    IEnumerator FadeAndChangeClip(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // 1. 기존 음악 볼륨 줄이기 (Fade Out)
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        // 2. 음악 에셋 교체 및 재생
        audioSource.Stop();
        audioSource.clip = newClip;

        if (newClip != null)
        {
            audioSource.Play();

            // 3. 새 음악 볼륨 키우기 (Fade In)
            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, startVolume, timer / duration);
                yield return null;
            }
        }

        audioSource.volume = startVolume; // 볼륨 원상 복구
    }
}
