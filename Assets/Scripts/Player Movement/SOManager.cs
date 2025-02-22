using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public SOPlayerControls playerControls;
    public SOBuildings buildingPrefabs;
    public SOShopItem[] shopItems;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
