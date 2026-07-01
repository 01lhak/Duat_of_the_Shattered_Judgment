using UnityEngine;

public class SaveLoadButtons : MonoBehaviour
{
    // =====================
    // SAVE
    // =====================
    public void Save1() => SaveWithSFX(1);
    public void Save2() => SaveWithSFX(2);
    public void Save3() => SaveWithSFX(3);
    public void Save4() => SaveWithSFX(4);

    // =====================
    // LOAD
    // =====================
    public void Load1() => LoadWithSFX(1);
    public void Load2() => LoadWithSFX(2);
    public void Load3() => LoadWithSFX(3);
    public void Load4() => LoadWithSFX(4);

    // =====================
    // SAVE 공통
    // =====================
    void SaveWithSFX(int slot)
    {
        PlayClick();

        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGame(slot);
        else
            Debug.LogWarning("SaveManager 없음");
    }

    // =====================
    // LOAD 공통
    // =====================
    void LoadWithSFX(int slot)
    {
        PlayClick();

        if (SaveManager.Instance != null)
            SaveManager.Instance.LoadGame(slot);
        else
            Debug.LogWarning("SaveManager 없음");
    }

    // =====================
    // 효과음
    // =====================
    void PlayClick()
    {
        if (SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();
    }
}