using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    public int targetDialogueIndex;
    public DialogueChoiceManager manager;

    Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // 선택지 클릭 효과음
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayClick();
        }

        // 선택지 처리
        manager.OnChoiceSelected(targetDialogueIndex);
    }
}