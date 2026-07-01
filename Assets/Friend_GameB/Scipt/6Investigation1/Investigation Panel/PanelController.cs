using UnityEngine;

public class PanelController : MonoBehaviour
{
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);

        if (InvestigationDialogueManager.Instance != null)
            InvestigationDialogueManager.Instance.HideDialogue();

        FadeInEffect.isInputAllowed = true;
    }
}