using UnityEngine;

// This thing should be placed on the camera.
public class ThingPlacer : MonoBehaviour
{
    public float placeDistance;
    Transform objectToPlaceTransform = null;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            objectToPlaceTransform = Instantiate(SOManager.instance.buildingPrefabs.flowerBed).transform;
        }
        RaycastHit hit;
        // if (Physics.Raycast(transform.position, transform.rotation, out hit, float.PositiveInfinity)) {
        //     GameObject hitGO = hit.collider.gameObject;
        // }
        // objectToPlaceTransform.position;
    }
}
