using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SOPlaceable", menuName = "Scriptable Objects/SOPlaceable")]
public class SOPlaceable : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public int price;
    public Sprite hotbarIcon;
    public GameObject itemPrefab;
    public AcceptSnap snapType;
    public bool placeableAnywhere;
    public float health;
    public bool targetable;
}

public enum ItemType {
    Default,
    FlowerBed,
    Turret,
}