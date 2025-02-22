using UnityEngine;

public enum ItemType {
    Default,
    FlowerItem,
    TurretItem,
}

[CreateAssetMenu(fileName = "SOShopItem", menuName = "Scriptable Objects/SOShopItem")]
public class SOShopItem : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite hotbarItem;
    public GameObject itemPrefab;
    public int price;
}
