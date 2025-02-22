using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerCamera.transform.forward, out hit, float.PositiveInfinity, 1 << 8)) {
                GameObject hitGO = hit.collider.gameObject;
                IInteractable interactableObj;
                if (hitGO.TryGetComponent(out interactableObj)) {
                    interactableObj.Interact();
                }
            }
        }
    }
}
