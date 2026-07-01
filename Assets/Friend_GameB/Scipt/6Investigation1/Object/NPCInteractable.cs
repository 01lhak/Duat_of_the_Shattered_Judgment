using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public string[] dialogues;
    public GameObject icon;

    private bool near;

    void Start()
    {
        if (icon != null)
            icon.SetActive(false);
    }

    void Update()
    {
        // 입력 차단 확인
        if (!FadeInEffect.isInputAllowed || GameManager.IsInputLocked)
            return;

        if (!near)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            // 매니저가 존재하는지 확인 후 로직 실행
            if (NPCDialogueManager.Instance != null)
            {
                if (!NPCDialogueManager.Instance.IsOpen())
                    NPCDialogueManager.Instance.StartDialogue(dialogues);
                else
                    NPCDialogueManager.Instance.Next();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        near = true;
        if (icon != null)
            icon.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        near = false;
        if (icon != null)
            icon.SetActive(false);

        // [핵심] Instance가 null일 수 있음을 대비하여 체크 추가 (NullReference 방지)
        if (NPCDialogueManager.Instance != null)
        {
            NPCDialogueManager.Instance.End();
        }
    }
}