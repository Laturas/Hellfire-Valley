using UnityEngine;

// This thing should be placed on the camera.
public class ThingPlacer : MonoBehaviour
{
    public float placeDistance;
    Transform objectToPlaceTransform = null;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (objectToPlaceTransform == null) {
                objectToPlaceTransform = Instantiate(SOManager.instance.buildingPrefabs.flowerBed).transform;
            } else {
                objectToPlaceTransform = null;
            }
        }
        if (objectToPlaceTransform != null) {
            Vector3 placePos = transform.position + transform.forward * placeDistance;
            Vector3 placeDir = Vector3.up;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, placeDistance)) {
                placePos = hit.point;
                placeDir = hit.normal;
            }
            objectToPlaceTransform.position = placePos;
            objectToPlaceTransform.up = placeDir;
        }
    }
}
