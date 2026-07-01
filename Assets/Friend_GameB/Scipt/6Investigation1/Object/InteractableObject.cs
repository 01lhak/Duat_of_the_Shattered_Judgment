using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [Header("대사")]
    public DialogueGroup[] dialogues;

    [Header("아이템")]
    public int itemID = -1;
    public ItemData itemData;

    [Header("UI")]
    public GameObject icon;

    [Header("사운드")]
    public AudioClip startSfx;
    public AudioClip pickupSfx;

    private bool near;
    private int dialogueIndex;
    private bool itemGiven;

    void Start()
    {
        if (icon != null)
            icon.SetActive(false);

        if (ItemDataManager.Instance != null &&
            ItemDataManager.Instance.HasItem(itemID))
        {
            itemGiven = true;
        }
    }

    void Update()
    {
        if (!FadeInEffect.isInputAllowed)
            return;

        if (GameManager.IsInputLocked)
            return;

        if (!near)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    void Interact()
    {
        if (InvestigationDialogueManager.Instance == null)
            return;

        if (InvestigationDialogueManager.Instance.IsOpen())
        {
            InvestigationDialogueManager.Instance.Next();

            if (!InvestigationDialogueManager.Instance.IsOpen())
            {
                dialogueIndex++;

                if (dialogueIndex >= dialogues.Length)
                    dialogueIndex = 0;

                GiveItem();
            }

            return;
        }

        if (dialogues.Length == 0)
            return;

        SFXManager.Instance?.Play(startSfx);

        InvestigationDialogueManager.Instance.StartDialogue(dialogues[dialogueIndex].lines);
    }

    void GiveItem()
    {
        if (itemGiven)
            return;

        if (itemID == -1)
            return;

        if (itemData == null)
            return;

        SFXManager.Instance?.Play(pickupSfx);

        ItemDataManager.Instance.GetItem(itemID);
        InventoryManager.Instance.AddItem(itemData);

        itemGiven = true;
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

        InvestigationDialogueManager.Instance?.End();
    }
}