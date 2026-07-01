using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public static System.Action OnSaveUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // =====================
    // SAVE
    // =====================
    public void SaveGame(int slot)
    {
        SaveData data = new SaveData();

        data.sceneName = SceneManager.GetActiveScene().name;

        data.currentChapter = GameManager.Instance.currentChapter;
        data.storyStep = GameManager.Instance.storyStep;
        data.selectedChoice = GameManager.Instance.selectedChoice;

        PlayerMove player = FindFirstObjectByType<PlayerMove>();
        if (player != null)
        {
            data.playerX = player.transform.position.x;
            data.playerY = player.transform.position.y;
        }

        if (ItemDataManager.Instance != null)
            data.itemIDs = ItemDataManager.Instance.GetAllItemIDs().ToArray();

        if (NPCDataManager.Instance != null)
            data.unlockedNPCs = NPCDataManager.Instance.GetUnlockedArray();

        data.saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string json = JsonUtility.ToJson(data, true);

        string path = Application.persistentDataPath + "/save" + slot + ".json";

        File.WriteAllText(path, json);

        // 이벤트 호출
        OnSaveUpdated?.Invoke();

        // 저장/로드 메뉴의 모든 슬롯 즉시 갱신
        SaveSlotInfo[] slots = Resources.FindObjectsOfTypeAll<SaveSlotInfo>();

        foreach (SaveSlotInfo info in slots)
        {
            if (info != null)
                info.Refresh();
        }

        Debug.Log("세이브 완료 (슬롯 " + slot + ")");
    }

    // =====================
    // LOAD
    // =====================
    public void LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/save" + slot + ".json";

        if (!File.Exists(path))
        {
            Debug.LogWarning("세이브 없음");
            return;
        }

        SaveData data =
            JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        StartCoroutine(LoadRoutine(data));
    }

    private IEnumerator LoadRoutine(SaveData data)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(data.sceneName);

        while (!op.isDone)
            yield return null;

        GameManager.Instance.currentChapter = data.currentChapter;
        GameManager.Instance.storyStep = data.storyStep;
        GameManager.Instance.selectedChoice = data.selectedChoice;

        PlayerMove player = FindFirstObjectByType<PlayerMove>();
        if (player != null)
        {
            player.transform.position =
                new Vector3(data.playerX, data.playerY, 0);
        }

        InventoryManager.Instance?.LoadItems(data.itemIDs);
        NPCDataManager.Instance?.LoadUnlocked(data.unlockedNPCs);
        ItemDataManager.Instance?.LoadCollected(data.itemIDs);

        Debug.Log("로드 완료");
    }
}