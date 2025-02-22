using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1000)]
public class InputManagerSingleton : MonoBehaviour
{
    public event Action OnInteract;
    private Controls controls;
    public static InputManagerSingleton instance;

	void Awake()
	{
        instance = this;
        DontDestroyOnLoad(this);
		controls = new Controls();
	}

	void OnInteractButtonPressed(InputAction.CallbackContext callbackContext) {
        OnInteract?.Invoke();
        Debug.Log("this");
	}

    void OnEnable() {
		controls.MainControls.Interact.performed += OnInteractButtonPressed;
        controls.MainControls.Enable();
    }
    void OnDisable() {
        controls.MainControls.Interact.performed -= OnInteractButtonPressed;
    }
}
