using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FadeInEffect : MonoBehaviour
{
    public static bool isInputAllowed = false;

    // 이번 게임 실행 동안 이미 실행한 씬 목록
    private static HashSet<int> playedScenes = new HashSet<int>();

    public Image startTextImage;

    [Header("페이드 시간")]
    public float fadeInTime = 1.0f;
    public float holdTime = 2.0f;
    public float fadeOutTime = 1.0f;

    [Header("시작 효과음")]
    public AudioClip startSFX;

    [Header("페이드 종료 후 나타날 버튼")]
    public GameObject nextButton;

    private float timer = 0.0f;

    private enum State
    {
        FadingIn,
        Holding,
        FadingOut,
        Finished
    }

    private State currentState = State.FadingIn;

    private int sceneIndex;

    void Start()
    {
        isInputAllowed = false;

        sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 이번 게임 실행 중 이미 실행한 씬이면 스킵
        if (playedScenes.Contains(sceneIndex))
        {
            if (startTextImage != null)
                startTextImage.gameObject.SetActive(false);

            if (nextButton != null)
                nextButton.SetActive(true);

            isInputAllowed = true;

            enabled = false;
            return;
        }

        SetAlpha(0f);

        if (nextButton != null)
            nextButton.SetActive(false);

        if (startSFX != null && SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(startSFX);
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentState)
        {
            case State.FadingIn:

                SetAlpha(Mathf.Clamp01(timer / fadeInTime));

                if (timer >= fadeInTime)
                {
                    currentState = State.Holding;
                    timer = 0f;
                }

                break;

            case State.Holding:

                if (timer >= holdTime)
                {
                    currentState = State.FadingOut;
                    timer = 0f;
                }

                break;

            case State.FadingOut:

                SetAlpha(1f - Mathf.Clamp01(timer / fadeOutTime));

                if (timer >= fadeOutTime)
                {
                    currentState = State.Finished;

                    if (nextButton != null)
                        nextButton.SetActive(true);

                    if (startTextImage != null)
                        startTextImage.gameObject.SetActive(false);

                    isInputAllowed = true;

                    // 이번 게임 실행 동안만 기억
                    playedScenes.Add(sceneIndex);
                }

                break;

            case State.Finished:
                break;
        }
    }

    private void SetAlpha(float alpha)
    {
        if (startTextImage == null)
            return;

        Color color = startTextImage.color;
        color.a = alpha;
        startTextImage.color = color;
    }
}