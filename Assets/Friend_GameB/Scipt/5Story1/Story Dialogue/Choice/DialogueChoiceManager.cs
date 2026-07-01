using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueChoiceManager : MonoBehaviour
{
    public GameObject choicePanel;

    public List<ChoiceButton> buttons;

    private StoryController controller;

    void Awake()
    {
        controller = FindObjectOfType<StoryController>();

        if (choicePanel != null)
            choicePanel.SetActive(false);
    }

    public void ShowChoices(DialogueLine line)
    {
        if (controller == null)
            controller = FindObjectOfType<StoryController>();

        choicePanel.SetActive(true);

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < line.choices.Count)
            {
                buttons[i].gameObject.SetActive(true);

                buttons[i].manager = this;

                buttons[i].targetDialogueIndex =
                    line.choices[i].jumpToLine;

                buttons[i]
                    .GetComponentInChildren<TextMeshProUGUI>()
                    .text = line.choices[i].choiceText;
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnChoiceSelected(int index)
    {
        choicePanel.SetActive(false);

        if (controller == null)
            controller = FindObjectOfType<StoryController>();

        controller.JumpToLine(index);
    }
}