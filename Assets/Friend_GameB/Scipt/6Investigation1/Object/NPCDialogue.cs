using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    // 각 캐릭터가 가질 자신만의 UI 요소를 인스펙터에서 연결합니다.
    public GameObject panel;
    public TextMeshProUGUI text;
    public Image standingImage; // (선택사항) 해당 캐릭터의 스탠딩 이미지

    [Header("대화 데이터")]
    [TextArea(3, 5)]
    public string[] lines; // 이 캐릭터가 대화할 내용들

    private int index;
    private bool isOpen = false;

    void Start()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    // 이 캐릭터의 대화를 시작하는 함수
    public void StartDialogue()
    {
        if (lines == null || lines.Length == 0) return;

        index = 0;
        isOpen = true;

        if (panel != null)
            panel.SetActive(true);

        Show();
    }

    // 다음 대사로 넘어가는 함수 (키보드 입력이나 버튼 클릭 시 호출)
    public void Next()
    {
        if (!isOpen || lines == null) return;

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
        {
            text.text = lines[index];
        }
    }

    public void End()
    {
        if (panel != null)
            panel.SetActive(false);

        index = 0;
        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
