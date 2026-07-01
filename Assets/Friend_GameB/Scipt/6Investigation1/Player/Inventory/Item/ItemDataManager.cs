using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public static ItemDataManager Instance;

    private HashSet<int> collected = new HashSet<int>();

    public List<ItemData> allItems = new List<ItemData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 아이템 획득
    public void GetItem(int id)
    {
        collected.Add(id);
    }

    // 이미 먹었는지 체크
    public bool HasItem(int id)
    {
        return collected.Contains(id);
    }

    // 저장용
    public List<int> GetAllItemIDs()
    {
        return new List<int>(collected);
    }

    // 로드용
    public void LoadCollected(int[] arr)
    {
        collected.Clear();

        if (arr == null) return;

        foreach (int id in arr)
            collected.Add(id);
    }

    public ItemData GetItemData(int id)
    {
        return allItems.Find(x => x.itemID == id);
    }
}