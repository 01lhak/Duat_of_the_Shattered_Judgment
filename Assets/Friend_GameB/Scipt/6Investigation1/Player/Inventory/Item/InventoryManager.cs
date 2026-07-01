using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventorySlot> slots;

    public GameObject itemInfoPanel;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDesc;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(false);

        ClearAllSlots();

        // ✔ 핵심 추가: 씬 이동 시 자동 복구
        RefreshInventoryFromData();
    }

    // =====================
    // 아이템 추가
    // =====================
    public void AddItem(ItemData item)
    {
        if (item == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null && slots[i].itemID == -1)
            {
                slots[i].SetItem(item);
                return;
            }
        }
    }

    // =====================
    // 슬롯 초기화
    // =====================
    void ClearAllSlots()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot != null)
                slot.ClearSlot();
        }
    }

    // =====================
    // 현재 아이템 ID 목록
    // =====================
    public List<int> GetItemIDs()
    {
        List<int> ids = new List<int>();

        foreach (InventorySlot slot in slots)
        {
            if (slot != null && slot.itemID != -1)
                ids.Add(slot.itemID);
        }

        return ids;
    }

    // =====================
    // 로드용
    // =====================
    public void LoadItems(int[] itemIDs)
    {
        ClearAllSlots();

        if (itemIDs == null) return;

        foreach (int id in itemIDs)
        {
            ItemData item = ItemDataManager.Instance.GetItemData(id);

            if (item != null)
                AddItem(item);
        }
    }

    // =====================
    // 씬 이동 복구 핵심
    // =====================
    public void RefreshInventoryFromData()
    {
        if (ItemDataManager.Instance == null) return;

        ClearAllSlots();

        foreach (int id in ItemDataManager.Instance.GetAllItemIDs())
        {
            ItemData item = ItemDataManager.Instance.GetItemData(id);

            if (item != null)
                AddItem(item);
        }
    }

    // =====================
    // 아이템 정보 UI
    // =====================
    public void ShowItemInfo(ItemData item)
    {
        if (item == null) return;

        if (itemInfoPanel != null)
            itemInfoPanel.SetActive(true);

        if (itemImage != null)
        {
            itemImage.sprite = item.icon;
            itemImage.color = Color.white;
        }

        if (itemName != null)
            itemName.text = item.itemName;

        if (itemDesc != null)
            itemDesc.text = item.description;
    }
}