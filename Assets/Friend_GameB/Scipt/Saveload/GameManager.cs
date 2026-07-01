using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentChapter = 1;
    public int storyStep = 0;
    public int selectedChoice = -1;

    // ======================
    // UI 상태
    // ======================

    // 설정창이 열려 있는지
    public static bool IsPauseOpen = false;

    // 인벤토리가 열려 있는지
    public static bool IsInventoryOpen = false;

    // 입력 잠금 여부
    public static bool IsInputLocked
    {
        get
        {
            return IsPauseOpen || IsInventoryOpen;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGameData()
    {
        currentChapter = 1;
        storyStep = 0;
        selectedChoice = -1;

        IsPauseOpen = false;
        IsInventoryOpen = false;
    }
}