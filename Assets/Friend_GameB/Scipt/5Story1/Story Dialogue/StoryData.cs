using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice
{
    public string choiceText;

    [Header("선택 시 이동할 대사 번호")]
    public int jumpToLine;
}

[System.Serializable]
public class DialogueLine
{
    [Header("캐릭터 패널 번호")]
    public int characterIndex;

    [Header("캐릭터 이미지")]
    public Sprite characterSprite;

    [TextArea(2,5)]
    public string text;

    [Header("선택지")]
    public List<Choice> choices = new List<Choice>();

    [Header("다음 이동할 대사 번호")]
    public int nextLine = -1;
}

[CreateAssetMenu(menuName = "Story/Data")]
public class StoryData : ScriptableObject
{
    public List<DialogueLine> lines = new List<DialogueLine>();
}