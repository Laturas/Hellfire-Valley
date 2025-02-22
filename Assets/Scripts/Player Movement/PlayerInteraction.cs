using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    private Collider[] plantColliders;

    private void Start() {
        plantColliders = new Collider[10];
    }
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
        if (Input.GetKeyDown(KeyCode.Q)) {
            int foundColliders = Physics.OverlapSphereNonAlloc(transform.position + SOManager.instance.playerControls.hoseSprayOrigin * transform.forward, SOManager.instance.playerControls.hoseSprayRadius, plantColliders, 1 << 8, QueryTriggerInteraction.Collide);

            for (int i = 0; i < foundColliders; i++) {
                Debug.Log(plantColliders[i].gameObject);
                Crop cropScript;
                if (plantColliders[i].gameObject.TryGetComponent(out cropScript)) {
                    cropScript.WaterThisPlant();
                }
            }
        }
    }
}
