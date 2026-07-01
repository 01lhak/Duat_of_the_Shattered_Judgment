using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [Header("설정 파일")]
    public AudioMixer masterMixer;

    [Header("슬라이더 오브젝트")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (masterMixer == null) return;

        // BGM 슬라이더 초기 위치 고정
        if (bgmSlider != null)
        {
            float bgmValue;
            if (masterMixer.GetFloat("BGM_Vol", out bgmValue))
            {
                // [중요] 이 코드가 있어야 볼륨 조절이 다시 먹힙니다.
                bgmSlider.onValueChanged.RemoveAllListeners();
                bgmSlider.value = Mathf.Pow(10, bgmValue / 20);
                // 코드로 다시 연결 (인스펙터 연결보다 이게 더 확실함)
                bgmSlider.onValueChanged.AddListener(SetBGMVolume);
            }
        }

        // SFX 슬라이더 초기 위치 고정
        if (sfxSlider != null)
        {
            float sfxValue;
            if (masterMixer.GetFloat("SFX_Vol", out sfxValue))
            {
                sfxSlider.onValueChanged.RemoveAllListeners();
                sfxSlider.value = Mathf.Pow(10, sfxValue / 20);
                sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            }
        }
    }

    // 슬라이더를 움직일 때 실행될 함수
    public void SetBGMVolume(float volume)
    {
        if (masterMixer == null) return;
        float vol = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20;
        masterMixer.SetFloat("BGM_Vol", vol);
    }

    public void SetSFXVolume(float volume)
    {
        if (masterMixer == null) return;
        float vol = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20;
        masterMixer.SetFloat("SFX_Vol", vol);
    }
}