using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NPCDialogueManager : MonoBehaviour
{
    public static NPCDialogueManager Instance;

    [Header("대화창 UI")]
    public GameObject panel;
    public TextMeshProUGUI text;
    public Image standingImage;

    [Header("선택지 UI")]
    public GameObject choicePanel;
    public Button moveButton;
    public Button cancelButton;

    private string[] lines;
    private int index;

    private string nextSceneName = "";
    private string[] currentCancelDialogues;
    private Sprite currentNpcSprite;

    // ★ [핵심 추가] 대화가 시작된 '바로 그 프레임'을 기억하여, 다른 NPC가 Next()를 동시에 호출하는 것을 차단합니다.
    private int dialogueStartFrame = -1;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (choicePanel != null) choicePanel.SetActive(false);

        if (moveButton != null)
        {
            moveButton.onClick.RemoveAllListeners();
            moveButton.onClick.AddListener(OnMoveSelected);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelSelected);
        }

        ResetDialogueState();
    }

    public void StartDialogue(string[] dialogueLines, Sprite npcSprite, string targetScene = "", string[] cancelLines = null)
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        ResetDialogueState();

        // ★ 현재 유니티의 실행 프레임 번호를 기록합니다.
        dialogueStartFrame = Time.frameCount;

        lines = dialogueLines;
        index = 0;
        nextSceneName = targetScene;
        currentCancelDialogues = cancelLines;
        currentNpcSprite = npcSprite;

        if (standingImage != null && npcSprite != null)
        {
            standingImage.sprite = npcSprite;
        }

        if (panel != null) panel.SetActive(true);
        if (choicePanel != null) choicePanel.SetActive(false);

        Show();
    }

    public void Next()
    {
        if (choicePanel != null && choicePanel.activeSelf) return;
        if (lines == null) return;

        // ★ [핵심 보안] 대화가 시작된 프레임과 현재 프레임이 똑같다면, 옆에 있던 다른 NPC가 신호를 교란한 것이므로 무조건 무시합니다.
        if (Time.frameCount == dialogueStartFrame)
            return;

        index++;

        if (index >= lines.Length)
        {
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                if (panel != null) panel.SetActive(false);
                if (choicePanel != null) choicePanel.SetActive(true);
                return;
            }

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
        if (panel != null) panel.SetActive(false);
        if (choicePanel != null) choicePanel.SetActive(false);

        lines = null;
        index = 0;
        nextSceneName = "";
        currentCancelDialogues = null;
        dialogueStartFrame = -1; // 리셋
    }

    public bool IsOpen()
    {
        bool isPanelOpen = panel != null && panel.activeSelf;
        bool isChoiceOpen = choicePanel != null && choicePanel.activeSelf;
        return isPanelOpen || isChoiceOpen;
    }

    void OnMoveSelected()
    {
        Debug.Log("★ [디버그] OnMoveSelected 함수 진입 성공!");

        string cleanedSceneName = nextSceneName.Trim();
        Debug.Log($"★ [디버그] 정제된 씬 이름: '{cleanedSceneName}'");

        if (!string.IsNullOrEmpty(cleanedSceneName))
        {
            Debug.Log($"★ [디버그] {cleanedSceneName} 씬 로드를 강제 실행합니다!");
            SceneManager.LoadScene(cleanedSceneName);
        }
        else
        {
            Debug.LogError(" [오류] nextSceneName이 비어있습니다.");
        }
    }

    void OnCancelSelected()
    {
        if (choicePanel != null) choicePanel.SetActive(false);

        if (currentCancelDialogues != null && currentCancelDialogues.Length > 0)
        {
            StartDialogue(currentCancelDialogues, currentNpcSprite, "", null);
        }
        else
        {
            End();
        }
    }

    private void ResetDialogueState()
    {
        lines = null;
        index = 0;
        nextSceneName = "";
        currentCancelDialogues = null;
        dialogueStartFrame = -1;

        if (panel != null) panel.SetActive(false);
        if (choicePanel != null) choicePanel.SetActive(false);
    }
}
