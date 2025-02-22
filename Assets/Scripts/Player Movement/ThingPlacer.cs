using UnityEngine;

// This thing should be placed on the camera.
public class ThingPlacer : MonoBehaviour
{
    private bool canPlace;
    Transform objectToPlaceTransform = null;
    void Start() {
        canPlace = false;
        overlaps = new Collider[10];
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (objectToPlaceTransform == null) {
                objectToPlaceTransform = Instantiate(SOManager.instance.buildingPrefabs.flowerBed).transform;
            } else {
                if (canPlace) {
                    objectToPlaceTransform = null;
                    canPlace = false;
                }
            }
        }
        if (objectToPlaceTransform != null) {
            PlacementLogic();
        }
    }

    private Collider[] overlaps;

    void OnDrawGizmos()
    {
        float distance = SOManager.instance.playerControls.placeDistance;
        float radius = SOManager.instance.playerControls.placeCheckRadius;
        Gizmos.DrawWireSphere(transform.position + transform.forward * distance, radius);
    }

    private void PlacementLogic() {
        float distance = SOManager.instance.playerControls.placeDistance;
        float radius = SOManager.instance.playerControls.placeCheckRadius;
        Vector3 placePos = transform.position + transform.forward * distance;
        Vector3 placeDir = Vector3.up;
        int snapHitCount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * distance, radius, overlaps, 1 << 6, QueryTriggerInteraction.Collide);
        
        for (int i = 0; i < snapHitCount; i++) {
            Collider hit = overlaps[i];
            Snappable snappableComponent;
            if (hit.gameObject.TryGetComponent(out snappableComponent)) {
                if (snappableComponent.IsValidSnap(AcceptSnap.Crops)) {
                    placePos = snappableComponent.gameObject.transform.position;
                    placeDir = snappableComponent.gameObject.transform.up;
                    canPlace = true;
                    break;
                }
            }
        }

        // RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.forward, out hit, placeDistance)) {
        //     placePos = hit.point;
        //     placeDir = hit.normal;
        //     Snappable snappableComponent;
        //     if (hit.collider.gameObject.TryGetComponent(out snappableComponent)) {
        //         if (snappableComponent.IsValidSnap(AcceptSnap.Crops)) {
        //             placePos = snappableComponent.gameObject.transform.position;
        //             placeDir = snappableComponent.gameObject.transform.up;
        //         }
        //     }
        // }
        objectToPlaceTransform.position = placePos;
        objectToPlaceTransform.up = placeDir;
    }
}
