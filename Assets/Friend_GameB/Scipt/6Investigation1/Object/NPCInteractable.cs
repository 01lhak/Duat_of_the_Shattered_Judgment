using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    // ★ 추가: 현재 전 세계(씬)에서 플레이어와 가장 가까워 '상호작용 버튼(F)을 누를 자격이 있는' 진짜 NPC 딱 한 명을 기억할 정적(static) 변수입니다.
    public static NPCInteractable CurrentTargetNPC;

    [Header("기본 대화 설정")]
    public string[] dialogues;
    public GameObject icon;
    public Sprite npcStandingSprite;

    [Header("씬 전환 설정 (필요한 NPC만 입력)")]
    public string targetSceneName = "";

    [Header("아직이야 선택 시 출력할 추가 대사")]
    [TextArea(2, 4)]
    public string[] cancelDialogues;

    private bool near;

    void Start()
    {
        if (icon != null)
            icon.SetActive(false);
    }

    void Update()
    {
        if (!FadeInEffect.isInputAllowed || GameManager.IsInputLocked)
            return;

        if (!near)
            return;

        // ★ 핵심 수정: 내가 현재 플레이어가 선택한 '진짜 상호작용 대상(CurrentTargetNPC)'이 아니라면 F키 입력을 완전히 무시합니다.
        // 이렇게 하면 맵에 NPC가 100명 있어도 내가 닿아있는 녀석 딱 한 명만 명령을 내립니다.
        if (CurrentTargetNPC != this && !NPCDialogueManager.Instance.IsOpen())
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (NPCDialogueManager.Instance != null)
            {
                if (!NPCDialogueManager.Instance.IsOpen())
                {
                    // 대화 시작할 때 이 NPC가 진짜 타겟임을 다시 한번 확실히 고정
                    CurrentTargetNPC = this;
                    NPCDialogueManager.Instance.StartDialogue(dialogues, npcStandingSprite, targetSceneName, cancelDialogues);
                }
                else
                {
                    // 대화 도중에는 현재 대화를 열어준 진짜 타겟 NPC만 Next()를 넘길 수 있도록 제한합니다.
                    if (CurrentTargetNPC == this)
                    {
                        NPCDialogueManager.Instance.Next();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        near = true;

        // 플레이어가 내 트리거 구역에 들어오면 전 세계 유일한 대화 타겟으로 나를 등록합니다.
        CurrentTargetNPC = this;

        if (icon != null) icon.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        near = false;

        // 내가 타겟인 상태에서 플레이어가 멀어졌다면 타겟 권한을 내려놓습니다.
        if (CurrentTargetNPC == this)
        {
            CurrentTargetNPC = null;
        }

        if (icon != null) icon.SetActive(false);

        if (NPCDialogueManager.Instance != null)
        {
            NPCDialogueManager.Instance.End();
        }
    }
}
