using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int itemID = -1;

    public Image iconImage;

    private ItemData itemData;

    void Awake()
    {
        if (iconImage == null)
            iconImage = GetComponentInChildren<Image>();
    }

    public void SetItem(ItemData item)
    {
        itemData = item;
        itemID = item.itemID;

        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
            iconImage.color = Color.white;
        }
    }

    public void ClearSlot()
    {
        itemData = null;
        itemID = -1;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }
    }

    // 🔥 클릭 핵심
    public void OnClick()
    {
        Debug.Log("슬롯 클릭됨");

        if (itemData == null)
            return;

        InventoryManager.Instance.ShowItemInfo(itemData);
    }
}