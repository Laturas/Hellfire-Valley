using UnityEngine;

public class EnableTester : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"OnEnable on {gameObject.name}");
    }

    private void Start()
    {
        Debug.Log($"Start on {gameObject.name}");
    }

    private void OnDisable()
    {
        Debug.Log($"OnDisable on {gameObject.name}");
    }

    private void Awake()
    {
        Debug.Log($"Awake on {gameObject.name}");
    }
}