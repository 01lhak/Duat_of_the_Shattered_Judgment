using UnityEngine;
using TMPro;
using System.IO;

public class SaveSlotInfo : MonoBehaviour
{
    public int slotNumber;
    public TMP_Text slotText;

    void OnEnable()
    {
        SaveManager.OnSaveUpdated += Refresh;
    }

    void OnDisable()
    {
        SaveManager.OnSaveUpdated -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        string path = Application.persistentDataPath + "/save" + slotNumber + ".json";

        if (!File.Exists(path))
        {
            slotText.text = "빈 슬롯";
            return;
        }

        SaveData data =
            JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        slotText.text =
            "챕터 " + data.currentChapter +
            "\n" + data.saveTime;
    }
}