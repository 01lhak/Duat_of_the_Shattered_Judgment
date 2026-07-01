using UnityEngine;
using TMPro;

public class InvestigationDialogueManager : MonoBehaviour
{
    public static InvestigationDialogueManager Instance;

    public GameObject panel;
    public TextMeshProUGUI text;

    private string[] lines;
    private int index;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        index = 0;

        panel.SetActive(true);
        Show();
    }

    // ⭐ 한 줄만 보여주기
    public void ShowDialogue(string line)
    {
        if (panel != null)
            panel.SetActive(true);

        text.text = line;
    }

    public void Next()
    {
        index++;

        if (index >= lines.Length)
        {
            End();
            return;
        }

        Show();
    }

    void Show()
    {
        text.text = lines[index];
    }

    public void End()
    {
        panel.SetActive(false);
        lines = null;
        index = 0;
    }

    // ⭐ UIItemClick, PanelController에서 사용
    public void HideDialogue()
    {
        End();
    }

    public bool IsOpen()
    {
        return panel != null && panel.activeSelf;
    }
}