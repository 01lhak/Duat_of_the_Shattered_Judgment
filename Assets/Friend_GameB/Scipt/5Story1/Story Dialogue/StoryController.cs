using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour
{
    public StoryData data;
    public NPCInfoPanel npcInfoPanel;
    public string nextSceneName;

    int lineIndex = 0;
    bool waitingChoice = false;

    void Start()
    {
        lineIndex = 0;
        ShowCurrentLine();
    }

    void Update()
    {
        // 입력 차단 조건들
        if (waitingChoice || SceneStartFade.isFading || GameManager.IsInputLocked)
            return;

        // UI 클릭 여부와 관계없이 마우스 클릭을 감지하도록 수정
        bool isInputTriggered = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (isInputTriggered)
        {
            var dm = Friend_DialogueManager.Instance;

            // 타이핑 중이면 Skip, 아니면 다음 대사(Next) 호출
            if (dm != null && dm.CurrentTypewriter != null && dm.CurrentTypewriter.IsTyping)
            {
                dm.CurrentTypewriter.Skip();
            }
            else
            {
                Next();
            }
        }
    }

    void ShowCurrentLine()
    {
        if (data == null || data.lines == null || lineIndex < 0 || lineIndex >= data.lines.Count)
        {
            EndStory();
            return;
        }

        DialogueLine line = data.lines[lineIndex];

        int npcID = GetNpcID(line.characterIndex);
        if (npcID >= 0 && npcInfoPanel != null)
        {
            if (NPCDataManager.Instance != null) NPCDataManager.Instance.Unlock(npcID);
            npcInfoPanel.gameObject.SetActive(true);
            npcInfoPanel.RefreshAll();
            npcInfoPanel.ShowNPC(npcID);
        }

        if (line.choices != null && line.choices.Count > 0)
        {
            waitingChoice = true;
            var choice = FindFirstObjectByType<DialogueChoiceManager>();
            if (choice != null) choice.ShowChoices(line);
            return;
        }

        if (Friend_DialogueManager.Instance != null)
        {
            Friend_DialogueManager.Instance.Show(line.characterIndex, line.text, line.characterSprite);
        }
    }

    public void Next()
    {
        if (data == null || data.lines == null) return;

        DialogueLine line = data.lines[lineIndex];
        lineIndex = (line.nextLine == -1) ? lineIndex + 1 : line.nextLine;

        if (lineIndex >= data.lines.Count) EndStory();
        else ShowCurrentLine();
    }

    public void JumpToLine(int index)
    {
        waitingChoice = false;
        lineIndex = index;
        ShowCurrentLine();
    }

    void EndStory()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    int GetNpcID(int characterIndex)
    {
        switch (characterIndex)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return 4;
            case 5: return 5;
            case 6: return 6;
            default: return -1;
        }
    }
}