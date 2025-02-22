using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }
}
