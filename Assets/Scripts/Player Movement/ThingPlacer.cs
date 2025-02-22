using UnityEngine;

// This thing should be placed on the camera.
public class ThingPlacer : MonoBehaviour
{
    private bool canPlace;
    [SerializeField] private SOPlaceable currentPlaceable;
    Transform objectToPlaceTransform = null;
    HotbarManager hotbar;
    void Start() {
        canPlace = false;
        overlaps = new Collider[10];
        hotbar = FindAnyObjectByType<HotbarManager>();
    }

    public void ChangeObjectToPlace(SOPlaceable placeThisThing) {
        currentPlaceable = placeThisThing;
        if (objectToPlaceTransform != null) {
            Destroy(objectToPlaceTransform.gameObject);
            objectToPlaceTransform = Instantiate(currentPlaceable.prefab).transform;
        }
    }

    void Update() {
        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     ChangeObjectToPlace(SOManager.instance.placeables[0]);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //     ChangeObjectToPlace(SOManager.instance.placeables[1]);
        // }
        if (currentPlaceable == null) {
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (objectToPlaceTransform == null) {
                objectToPlaceTransform = Instantiate(currentPlaceable.prefab).transform;
            } else {
                if (canPlace) {
                    objectToPlaceTransform = null;
                    canPlace = false;
                }
            }
        }
        if (objectToPlaceTransform != null) {
            if (currentPlaceable.placeableAnywhere) {
                PlacementLogicAnywhere();
            } else {
                PlacementLogicSnap();
            }
        }
    }

    private Collider[] overlaps;

    // void OnDrawGizmos()
    // {
    //     float distance = SOManager.instance.playerControls.placeDistance;
    //     float radius = SOManager.instance.playerControls.placeCheckRadius;
    //     Gizmos.DrawWireSphere(transform.position + transform.forward * distance, radius);
    // }

    private void PlacementLogicAnywhere() {
        RaycastHit hit;
        float distance = SOManager.instance.playerControls.placeDistance;
        Vector3 placePos = transform.position + transform.forward * distance;
        Vector3 placeDir = Vector3.up;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance, 1 << 0)) {
            placePos = hit.point;
            placeDir = hit.normal;
            canPlace = Vector3.Angle(placeDir, Vector3.up) < SOManager.instance.playerControls.steepestBuildingPlaceAngle;
            Debug.Log(Vector3.Angle(placeDir, Vector3.up));
        } else {
            canPlace = false;
        }
        objectToPlaceTransform.position = placePos;
        objectToPlaceTransform.up = placeDir;
    }

    private void PlacementLogicSnap() {
        float distance = SOManager.instance.playerControls.placeDistance;
        float radius = SOManager.instance.playerControls.placeCheckRadius;
        Vector3 placePos = transform.position + transform.forward * distance;
        Vector3 placeDir = Vector3.up;
        int snapHitCount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * distance, radius, overlaps, 1 << 6, QueryTriggerInteraction.Collide);
        
        for (int i = 0; i < snapHitCount; i++) {
            Collider hit = overlaps[i];
            Snappable snappableComponent;
            if (hit.gameObject.TryGetComponent(out snappableComponent)) {
                if (snappableComponent.IsValidSnap(currentPlaceable.snapType)) {
                    placePos = snappableComponent.gameObject.transform.position;
                    placeDir = snappableComponent.gameObject.transform.up;
                    canPlace = true;
                    break;
                } else {
                    canPlace = false;
                }
            }
        }

        objectToPlaceTransform.position = placePos;
        objectToPlaceTransform.up = placeDir;
    }
}
