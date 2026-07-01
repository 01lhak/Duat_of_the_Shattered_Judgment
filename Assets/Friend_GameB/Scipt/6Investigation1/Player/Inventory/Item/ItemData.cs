using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public string description;
    public Sprite icon;
}