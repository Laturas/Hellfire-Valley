using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager instance;
    public SOPlayerControls playerControls;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
