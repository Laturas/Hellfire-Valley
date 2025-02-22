using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public SOPlayerControls playerControls;
    public SOBuildings buildingPrefabs;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
