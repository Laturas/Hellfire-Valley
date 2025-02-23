using UnityEngine;

// Internally builds a bitmap to easily compare snapping.
public enum AcceptSnap {
    None = 0,
    Turrets = 1,
    GrowMods = 2,
    AllBuildings = 3,
    Crops = 4,
    All = 7,
}

public class Snappable : MonoBehaviour
{
    [SerializeField] private AcceptSnap thisCanSnapToMe;
    public bool occupied {get; private set;}

    public bool IsValidSnap(AcceptSnap objectType) {
        return (thisCanSnapToMe & objectType) > 0;
    }

    public void Occupy()
    {
        occupied = true;
    }

    public void Release()
    {
        occupied = false;
    }
}
