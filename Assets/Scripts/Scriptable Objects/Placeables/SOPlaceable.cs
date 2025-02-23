using UnityEngine;

[CreateAssetMenu(fileName = "SOPlaceable", menuName = "Scriptable Objects/SOPlaceable")]
public class SOPlaceable : ScriptableObject
{
    public bool placeableAnywhere;
    public AcceptSnap snapType;
    public GameObject prefab;
    public bool targetable;
    public float health;
    public ItemType type;
}
