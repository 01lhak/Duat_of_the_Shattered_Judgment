[System.Serializable]
public class SaveData
{
    public string sceneName;

    public int currentChapter;
    public int storyStep;
    public int selectedChoice;

    public float playerX;
    public float playerY;

    public int[] itemIDs;
    public int[] unlockedNPCs;

    // ✔ 핵심 추가: 이미 먹은 아이템
    public int[] collectedItemIDs;

    public string saveTime;
}