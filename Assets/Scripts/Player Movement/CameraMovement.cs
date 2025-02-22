using UnityEngine;
using UnityEngine.InputSystem;

// Controls
public class CameraMovement : MonoBehaviour
{
	[SerializeField] private SOPlayerControls movementSettings;
	Vector2 currentLook;
	private Camera mainCamera;

    void Start()
    {
		mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
	{
		Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		mouseInput.x *= movementSettings.Xsensitivity;
		mouseInput.y *= movementSettings.Ysensitivity;

		currentLook.x += mouseInput.x;
		currentLook.y = Mathf.Clamp(currentLook.y += mouseInput.y, -90, 90);

		transform.localRotation = Quaternion.AngleAxis(-currentLook.y, Vector3.right);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y);
		transform.parent.transform.localRotation = Quaternion.Euler(0, currentLook.x, 0);
	}
}
