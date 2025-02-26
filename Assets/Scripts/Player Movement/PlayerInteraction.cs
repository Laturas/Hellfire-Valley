using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public Animator wateringCan;
    public GameObject water;
    private Collider[] plantColliders;

    private void Start() {
        plantColliders = new Collider[10];
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerCamera.transform.forward, out hit, float.PositiveInfinity, 1 << 13)) {
                GameObject hitGO = hit.collider.gameObject;
                IInteractable interactableObj;
                if (hitGO.TryGetComponent(out interactableObj)) {
                    interactableObj.Interact();
                }
            }
        }
        if (Input.GetKey(KeyCode.Q)) {
            wateringCan.SetBool("Watering", true);
            water.SetActive(true);
            int foundColliders = Physics.OverlapSphereNonAlloc(transform.position + SOManager.instance.playerControls.hoseSprayOrigin * transform.forward, SOManager.instance.playerControls.hoseSprayRadius, plantColliders, 1 << 13, QueryTriggerInteraction.Collide);

            for (int i = 0; i < foundColliders; i++) {
                Crop cropScript;
                if (plantColliders[i].gameObject.TryGetComponent(out cropScript)) {
                    cropScript.WaterThisPlant();
                }
            }
        } else {
            wateringCan.SetBool("Watering", false);
            water.SetActive(false);
        }
    }
}
