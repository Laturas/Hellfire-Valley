using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public SOPlayerControls playerControls;
    public SOBuildings buildingPrefabs;
    public SOShopItem[] shopItems;
    public SOPlaceable[] placeables;
    public SOTurret[] turretTypes;
    public SOWorldHudIcons hudIcons;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
