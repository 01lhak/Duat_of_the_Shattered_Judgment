using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Profile")]
public class NPCProfileData : ScriptableObject
{
    [Header("고유 ID (절대 중복 금지)")]
    public int npcID;

    [Header("큰 이미지")]
    public Sprite portrait;

    [Header("버튼 아이콘")]
    public Sprite icon;

    [Header("이름")]
    public string npcName;

    [Header("간단 정보")]
    public string age;
    public string gender;
    public string job;
    public string affiliation;

    [TextArea(3, 10)]
    public string description;

    [Header("코멘트")]
    public Sprite commentPortrait;

    [TextArea(2, 5)]
    public string comment;
}