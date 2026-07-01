using UnityEngine;
using TMPro;

public class InventoryTabManager : MonoBehaviour
{
    public GameObject inventoryWindow;
    public GameObject NPCInfoPanel;
    public GameObject ItemPanel;

    [Header("버튼 텍스트")]
    public TextMeshProUGUI npcButtonText;
    public TextMeshProUGUI itemButtonText;

    // 기본 / 선택 색상
    private readonly Color normalColor = new Color32(0x39, 0x27, 0x13, 255);   // #392713
    private readonly Color selectedColor = new Color32(0x9D, 0x53, 0x00, 255); // #9D5300

    void Awake()
    {
        if (inventoryWindow == null || NPCInfoPanel == null || ItemPanel == null)
        {
            Debug.LogError("InventoryTabManager 연결 누락");
        }
    }

    void Start()
    {
        SafeInit();
    }

    void SafeInit()
    {
        if (inventoryWindow == null || NPCInfoPanel == null || ItemPanel == null)
            return;

        inventoryWindow.SetActive(false);
        NPCInfoPanel.SetActive(true);
        ItemPanel.SetActive(false);

        GameManager.IsInventoryOpen = false;

        // 기본은 NPC 탭 선택
        UpdateButtonTextColor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            // 설정창이 열려 있으면 인벤토리 열기 금지
            if (GameManager.IsPauseOpen)
                return;

            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryWindow == null)
            return;

        bool open = !inventoryWindow.activeSelf;

        inventoryWindow.SetActive(open);

        GameManager.IsInventoryOpen = open;

        if (open)
        {
            if (SFXManager.Instance != null)
                SFXManager.Instance.PlayInventoryOpen();

            ShowNPCInfo(false);
        }
    }

    public void CloseInventory()
    {
        if (inventoryWindow != null)
            inventoryWindow.SetActive(false);

        GameManager.IsInventoryOpen = false;
    }

    public void ShowNPCInfo(bool playSfx = true)
    {
        if (NPCInfoPanel == null || ItemPanel == null)
            return;

        NPCInfoPanel.SetActive(true);
        ItemPanel.SetActive(false);

        UpdateButtonTextColor(true);

        if (playSfx && SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();
    }

    public void ShowItem(bool playSfx = true)
    {
        if (NPCInfoPanel == null || ItemPanel == null)
            return;

        NPCInfoPanel.SetActive(false);
        ItemPanel.SetActive(true);

        UpdateButtonTextColor(false);

        if (playSfx && SFXManager.Instance != null)
            SFXManager.Instance.PlayClick();
    }

    private void UpdateButtonTextColor(bool npcSelected)
    {
        if (npcButtonText != null)
            npcButtonText.color = npcSelected ? selectedColor : normalColor;

        if (itemButtonText != null)
            itemButtonText.color = npcSelected ? normalColor : selectedColor;
    }
}