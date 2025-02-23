using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public static GlobalSettings instance;
    public float sensitivityModifier = 1f;
    public float volumeModifier = 1f;

    void Awake()
    {
        // This ACTUALLY has to be don't destroy on load, otherwise
        // player settings would get reset between attempts.
        if (instance != null) {Destroy(this); return;}
        instance = this;
        DontDestroyOnLoad(gameObject);
        sensitivityModifier = 1f;
        volumeModifier = 1f;
    }
}
