using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("패널 설정")]
    public GameObject settingsPanel;
    public GameObject soundPanel;
    public GameObject savePanel;
    public GameObject loadPanel;

    [Header("버튼 텍스트")]
    public TextMeshProUGUI soundButtonText;
    public TextMeshProUGUI saveButtonText;
    public TextMeshProUGUI loadButtonText;

    [Header("SFX")]
    public AudioClip escOpenSfx;
    public AudioClip escCloseSfx;

    private readonly Color normalColor = Color.white;
    private readonly Color selectedColor = new Color32(0xD5, 0x91, 0x4A, 255);

    void Start()
    {
        InitPanels();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameManager.IsInventoryOpen)
                return;

            ToggleSettings();
        }
    }

    void InitPanels()
    {
        if (settingsPanel == null)
            return;

        settingsPanel.SetActive(false);

        if (soundPanel != null) soundPanel.SetActive(true);
        if (savePanel != null) savePanel.SetActive(false);
        if (loadPanel != null) loadPanel.SetActive(false);

        GameManager.IsPauseOpen = false;

        UpdateButtonTextColor(soundPanel);
    }

    public void ToggleSettings()
    {
        if (settingsPanel == null)
            return;

        bool newState = !settingsPanel.activeSelf;
        settingsPanel.SetActive(newState);

        GameManager.IsPauseOpen = newState;

        // 🔥 ESC 전용 사운드만 재생
        if (newState)
            PlayEscOpen();
        else
            PlayEscClose();

        // ❌ ShowSoundPanel() 쓰지 않음 (여기 중요)
        if (newState)
            OpenSoundPanelNoSfx();
    }

    // ======================
    // 🔥 SFX 없는 패널 전환
    // ======================
    void OpenSoundPanelNoSfx()
    {
        SetActivePanel(soundPanel);
    }

    void SetActivePanel(GameObject target)
    {
        if (soundPanel != null)
            soundPanel.SetActive(target == soundPanel);

        if (savePanel != null)
            savePanel.SetActive(target == savePanel);

        if (loadPanel != null)
            loadPanel.SetActive(target == loadPanel);

        UpdateButtonTextColor(target);
    }

    void UpdateButtonTextColor(GameObject target)
    {
        if (soundButtonText != null)
            soundButtonText.color = (target == soundPanel) ? selectedColor : normalColor;

        if (saveButtonText != null)
            saveButtonText.color = (target == savePanel) ? selectedColor : normalColor;

        if (loadButtonText != null)
            loadButtonText.color = (target == loadPanel) ? selectedColor : normalColor;
    }

    public void ShowSoundPanel()
    {
        PlayClick();
        SetActivePanel(soundPanel);
    }

    public void ShowSavePanel()
    {
        PlayClick();
        SetActivePanel(savePanel);
    }

    public void ShowLoadPanel()
    {
        PlayClick();
        SetActivePanel(loadPanel);
    }

    public void OnClickSoundButton() => ShowSoundPanel();
    public void OnClickSaveButton() => ShowSavePanel();
    public void OnClickLoadButton() => ShowLoadPanel();

    public void OnClickBackButton()
    {
        StartCoroutine(BackToMainCoroutine());
    }

    IEnumerator BackToMainCoroutine()
    {
        PlayClick();
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }

    void PlayClick()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();
    }

    void PlayEscOpen()
    {
        if (SFXManager.Instance != null && escOpenSfx != null)
            SFXManager.Instance.PlaySFX(escOpenSfx);
    }

    void PlayEscClose()
    {
        if (SFXManager.Instance != null && escCloseSfx != null)
            SFXManager.Instance.PlaySFX(escCloseSfx);
    }
}