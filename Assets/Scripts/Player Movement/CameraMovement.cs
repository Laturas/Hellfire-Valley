using UnityEngine;

// Controls
public class CameraMovement : MonoBehaviour
{
	Vector2 currentLook;
	private Camera mainCamera;
	[SerializeField] private Transform cameraFollowTransform;

    void Start()
    {
		mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
	{
		transform.position = cameraFollowTransform.position;
		if (Time.timeScale < 0.01f) {return;}
		Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		mouseInput.x *= SOManager.instance.playerControls.Xsensitivity * GlobalSettings.instance.sensitivityModifier;
		mouseInput.y *= SOManager.instance.playerControls.Ysensitivity * GlobalSettings.instance.sensitivityModifier;

		currentLook.x += mouseInput.x;
		currentLook.y = Mathf.Clamp(currentLook.y + mouseInput.y, -90, 90);

		//transform.localRotation = Quaternion.AngleAxis(-currentLook.y, Vector3.right);
		transform.localRotation = Quaternion.Euler(-currentLook.y, currentLook.x, 0);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y);
		//transform.parent.transform.localRotation = Quaternion.Euler(0, currentLook.x, 0);
	}
}
