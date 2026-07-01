using UnityEngine;
using TMPro;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance;

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
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        lines = dialogueLines;
        index = 0;

        if (panel != null)
            panel.SetActive(true);

        Show();
    }

    public void Next()
    {
        if (lines == null) return;

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
        if (text != null && lines != null && index < lines.Length)
            text.text = lines[index];
    }

    public void End()
    {
        if (panel != null)
            panel.SetActive(false);

        lines = null;
        index = 0;
    }

    public bool IsOpen()
    {
        return panel != null && panel.activeSelf;
    }
}