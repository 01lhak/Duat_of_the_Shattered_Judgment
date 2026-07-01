using UnityEngine;
using UnityEngine.InputSystem;

public class InvestigationPanel : MonoBehaviour
{
    public GameObject interactIcon;

    [Header("열 패널")]
    public GameObject investigationPanel;

    private bool isPlayerNearby = false;

    private void Start()
    {
        if (interactIcon != null)
            interactIcon.SetActive(false);

        if (investigationPanel != null)
            investigationPanel.SetActive(false);
    }

    private void Update()
    {
        // 인트로 중에는 입력 차단
        if (!FadeInEffect.isInputAllowed)
            return;

        // 설정창 또는 인벤토리가 열려 있으면 입력 차단
        if (GameManager.IsInputLocked)
            return;

        if (isPlayerNearby &&
            Keyboard.current != null &&
            Keyboard.current.fKey.wasPressedThisFrame)
        {
            OpenPanel();
        }
    }

    private void OpenPanel()
    {
        if (investigationPanel != null)
        {
            investigationPanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (interactIcon != null)
                interactIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isPlayerNearby = true;

        if (interactIcon != null)
            interactIcon.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isPlayerNearby = false;

        if (interactIcon != null)
            interactIcon.SetActive(false);
    }
}