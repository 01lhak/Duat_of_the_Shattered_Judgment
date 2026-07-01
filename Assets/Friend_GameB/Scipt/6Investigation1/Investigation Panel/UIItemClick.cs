using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemClick : MonoBehaviour, IPointerClickHandler
{
    [Header("조사 대사")]
    [TextArea(2, 5)]
    public string[] dialogues;

    [Header("획득 아이템")]
    public ItemData item;

    [Header("조사 효과음")]
    public AudioClip investigateSFX;

    [Header("아이템 획득 효과음")]
    public AudioClip itemGetSFX;

    private bool isCollected = false;
    private int currentIndex = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InvestigationDialogueManager.Instance == null)
            return;

        // 대사창이 닫혀있으면 첫 대사 출력
        if (!InvestigationDialogueManager.Instance.IsOpen())
        {
            currentIndex = 0;

            if (SFXManager.Instance != null && investigateSFX != null)
                SFXManager.Instance.PlaySFX(investigateSFX);

            if (dialogues != null && dialogues.Length > 0)
            {
                InvestigationDialogueManager.Instance.ShowDialogue(
                    dialogues[currentIndex]
                );
            }

            return;
        }

        // 다음 대사
        currentIndex++;

        if (currentIndex >= dialogues.Length)
        {
            InvestigationDialogueManager.Instance.HideDialogue();

            // 마지막 대사 후 아이템 지급
            if (!isCollected && item != null)
            {
                if (ItemDataManager.Instance != null)
                    ItemDataManager.Instance.GetItem(item.itemID);

                if (InventoryManager.Instance != null)
                    InventoryManager.Instance.AddItem(item);

                if (SFXManager.Instance != null && itemGetSFX != null)
                    SFXManager.Instance.PlaySFX(itemGetSFX);

                isCollected = true;

                Debug.Log("아이템 획득 : " + item.itemName);
            }

            currentIndex = 0;
            return;
        }

        InvestigationDialogueManager.Instance.ShowDialogue(
            dialogues[currentIndex]
        );
    }
}