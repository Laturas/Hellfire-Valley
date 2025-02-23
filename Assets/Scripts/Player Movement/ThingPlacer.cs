using System;
using UnityEngine;

// This thing should be placed on the camera.
public class ThingPlacer : MonoBehaviour
{
    private bool canPlace;
    [SerializeField] private SOPlaceable currentPlaceable;
    private HotbarManager hotbar;
    private PreviewManager previewManager;
    private Collider[] overlaps;
    private Vector3 placePosition = Vector3.zero;
    private Snappable placeToSnapTo;
    
    private void Start() {
        canPlace = false;
        overlaps = new Collider[10];
        hotbar ??= FindAnyObjectByType<HotbarManager>();
        previewManager ??= FindFirstObjectByType<PreviewManager>();
    }

    private void OnEnable()
    {
        hotbar ??= FindAnyObjectByType<HotbarManager>();
        previewManager ??= FindFirstObjectByType<PreviewManager>();
        hotbar.OnItemSelected += OnItemSelected;
    }

    private void OnDisable()
    {
        hotbar.OnItemSelected -= OnItemSelected;
    }

    private void OnItemSelected(SOPlaceable item)
    {
        if (!item)
        {
            currentPlaceable = null;
            previewManager.DisablePreview();
        }
        else ChangeObjectToPlace(item);
    }

    public void ChangeObjectToPlace(SOPlaceable placeThisThing) {
        currentPlaceable = placeThisThing;
        previewManager.PlacePreview(placeThisThing, transform.position, true);
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     ChangeObjectToPlace(SOManager.instance.placeables[0]);
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //     ChangeObjectToPlace(SOManager.instance.placeables[1]);
        // }
        if (!currentPlaceable) {
            return;
        }
        
        if (Input.GetMouseButtonDown(0)) {
            if (canPlace) {
                //objectToPlaceTransform = null;
                previewManager.DisablePreview();
                var gobj = Instantiate(currentPlaceable.itemPrefab, placePosition, Quaternion.identity);
                
                if (placeToSnapTo) gobj.GetComponent<AbstractPlaceable>().OccupySnappable(placeToSnapTo);
                if (!hotbar.RemoveItem(currentPlaceable)) Debug.LogWarning("Tried removing item that doesn't exist");
                canPlace = false;
            }
        }
        if (previewManager.IsPreviewActive() && currentPlaceable.placeableAnywhere) {
            PlacementLogicAnywhere();
        } else if (previewManager.IsPreviewActive()){
            PlacementLogicSnap();
        }
    }
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
            //Debug.Log(Vector3.Angle(placeDir, Vector3.up));
        } else {
            canPlace = false;
        }
        placeToSnapTo = null;
        PlaceObject(placePos, placeDir);
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
            if (hit.gameObject.TryGetComponent(out snappableComponent))
            {
                if (snappableComponent.IsValidSnap(currentPlaceable.snapType) && !snappableComponent.occupied) {
                    placePos = snappableComponent.gameObject.transform.position;
                    placeDir = snappableComponent.gameObject.transform.up;
                    placeToSnapTo = snappableComponent;
                    canPlace = true;
                    PlaceObject(placePos, placeDir);
                    return;
                }
            }
        }

        placeToSnapTo = null;
        canPlace = false;
        PlaceObject(placePos, placeDir);
    }

    private void PlaceObject(Vector3 position, Vector3 direction)
    {
        placePosition = position;
        previewManager.PlacePreview(currentPlaceable, position, canPlace);
    }
}
