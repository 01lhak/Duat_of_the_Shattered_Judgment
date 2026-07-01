using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NPCInfoPanel : MonoBehaviour
{
    [Header("NPC 데이터")]
    public List<NPCProfileData> allNPCs;

    [Header("버튼 이미지")]
    public List<Image> npcButtonImages;

    [Header("잠금 이미지 (버튼)")]
    public Sprite buttonLockedSprite;

    [Header("잠금 이미지 (프로필)")]
    public Sprite profileLockedSprite;

    [Header("UI")]
    public Image profileImage;
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI ageText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI jobText;
    public TextMeshProUGUI affiliationText;

    public TextMeshProUGUI descriptionText;

    public Image commentProfileImage;
    public TextMeshProUGUI commentText;

    void Start()
    {
        RefreshAll();

        if (allNPCs != null && allNPCs.Count > 0)
            ShowNPC(allNPCs[0].npcID);
    }

    // =========================
    // UI 전체 갱신
    // =========================
    public void RefreshAll()
    {
        RefreshButtons();
    }

    void RefreshButtons()
    {
        if (allNPCs == null || npcButtonImages == null) return;

        for (int i = 0; i < allNPCs.Count; i++)
        {
            if (i >= npcButtonImages.Count) continue;

            NPCProfileData npc = allNPCs[i];

            bool unlocked =
                NPCDataManager.Instance != null &&
                NPCDataManager.Instance.IsUnlocked(npc.npcID);

            npcButtonImages[i].sprite =
                unlocked ? npc.icon : buttonLockedSprite;
        }
    }

    // =========================
    // NPC 표시
    // =========================
    public void ShowNPC(int npcID)
    {
        if (NPCDataManager.Instance == null) return;

        NPCProfileData npc =
            allNPCs.Find(x => x.npcID == npcID);

        if (npc == null)
        {
            ShowLocked();
            return;
        }

        if (!NPCDataManager.Instance.IsUnlocked(npcID))
        {
            ShowLocked();
            return;
        }

        profileImage.sprite = npc.portrait;
        nameText.text = npc.npcName;

        ageText.text = npc.age;
        genderText.text = npc.gender;
        jobText.text = npc.job;
        affiliationText.text = npc.affiliation;

        descriptionText.text = npc.description;

        commentProfileImage.sprite = npc.commentPortrait;
        commentText.text = npc.comment;
    }

    void ShowLocked()
    {
        profileImage.sprite = profileLockedSprite;

        nameText.text = "???";
        ageText.text = "???";
        genderText.text = "???";
        jobText.text = "???";
        affiliationText.text = "???";

        descriptionText.text = "아직 만나지 못한 인물입니다.";

        commentProfileImage.sprite = profileLockedSprite;
        commentText.text = "???";
    }
}