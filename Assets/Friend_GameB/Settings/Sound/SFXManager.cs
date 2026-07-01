using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource sfxSource;

    [Header("효과음")]
    public AudioClip buttonClickSFX;
    public AudioClip itemGetSFX;
    public AudioClip inventoryOpenSFX;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        sfxSource = GetComponent<AudioSource>();

        // 🔥 안전장치 (AudioSource 없을 때 자동 생성)
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    // =========================
    // 기본 재생 함수
    // =========================
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }

    // =========================
    // 편의 함수
    // =========================
    public void PlayClick()
    {
        PlaySFX(buttonClickSFX);
    }

    public void PlayItemGet()
    {
        PlaySFX(itemGetSFX);
    }

    public void PlayInventoryOpen()
    {
        PlaySFX(inventoryOpenSFX);
    }

    // =========================
    // 🔥 외부 확장용 (추천)
    // =========================
    public void Play(AudioClip clip)
    {
        PlaySFX(clip);
    }
}