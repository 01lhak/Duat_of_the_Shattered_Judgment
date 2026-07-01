using UnityEngine;

public class UIBlocker : MonoBehaviour
{
    public static UIBlocker Instance;

    public GameObject blockerPanel;

    void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        if (blockerPanel != null)
            blockerPanel.SetActive(true);
    }

    public void Hide()
    {
        if (blockerPanel != null)
            blockerPanel.SetActive(false);
    }

    public bool IsUIOpen()
    {
        return blockerPanel != null && blockerPanel.activeSelf;
    }
}