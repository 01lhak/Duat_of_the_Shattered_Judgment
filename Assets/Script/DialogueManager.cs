using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class DialogueManager : MonoBehaviour
{
    public TypewriterEffect typewriter;

    public Image leftCharacter;
    public Image rightCharacter;

    public TMP_Text nameText;

    public Sprite[] leftExpressions;
    public Sprite[] rightExpressions;

    public string[] dialogues;
    public string[] characterNames;

    private int index = 0;

    void Start()
    {
        ShowDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        // 대사 출력
        typewriter.StartTyping(dialogues[index]);

        // 이름 출력
        if (nameText != null && characterNames.Length > index)
        {
            nameText.text = characterNames[index];
        }

        // 메제드가 말할 때
        if (characterNames[index] == "메제드")
        {
            leftCharacter.color = Color.white;
            rightCharacter.color = Color.gray;

            if (leftExpressions.Length > index)
            {
                leftCharacter.sprite = leftExpressions[index];
            }
        }

        // 아누비스가 말할 때
        else if (characterNames[index] == "아누비스")
        {
            leftCharacter.color = Color.gray;
            rightCharacter.color = Color.white;

            if (rightExpressions.Length > index)
            {
                rightCharacter.sprite = rightExpressions[index];
            }
        }
    }

    void NextDialogue()
    {
        // 👉 타이핑 중이면 스킵
        if (typewriter.IsTyping())
        {
            typewriter.SkipTyping(dialogues[index]);
            return;
        }

        // 👉 다음 대사
        if (index < dialogues.Length - 1)
        {
            index++;
            ShowDialogue();
        }
    }
    void HighlightSpeaker(bool leftSpeaking)
    {
        if (leftSpeaking)
        {
            leftCharacter.color = Color.white;
            rightCharacter.color = Color.gray;
        }
        else
        {
            leftCharacter.color = Color.gray;
            rightCharacter.color = Color.white;
        }
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}