using System.Collections.Generic;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    public static NPCDataManager Instance;

    HashSet<int> unlocked = new HashSet<int>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 기본 해금 NPC
        unlocked.Add(0);
        unlocked.Add(1);
    }

    public void Unlock(int npcID)
    {
        unlocked.Add(npcID);
    }

    public bool IsUnlocked(int npcID)
    {
        return unlocked.Contains(npcID);
    }

    // =========================
    // SAVE/LOAD용
    // =========================

    public int[] GetUnlockedArray()
    {
        return new List<int>(unlocked).ToArray();
    }

    public void LoadUnlocked(int[] ids)
    {
        unlocked.Clear();

        // 기본 해금 NPC
        unlocked.Add(0);
        unlocked.Add(1);

        if (ids == null)
            return;

        foreach (int id in ids)
            unlocked.Add(id);
    }
}