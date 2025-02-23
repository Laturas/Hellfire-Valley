using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public SOPlayerControls playerControls;
    public SOBuildings buildingPrefabs;
    public SOPlaceable[] placeables;
    public SOTurret[] turretTypes;
    public SOWorldHudIcons hudIcons;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
