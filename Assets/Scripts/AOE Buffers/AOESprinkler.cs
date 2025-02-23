using UnityEngine;

public class AOESprinkler : MonoBehaviour, IAOEBuff
{
    [SerializeField] private float radius = 5f;

    public void AlertCrops()
    {
        Collider[] crops = Physics.OverlapSphere(transform.position, radius, 1 << 13, QueryTriggerInteraction.Collide);

        foreach (Collider cropCollider in crops) {
            Crop crop;
            if (cropCollider.TryGetComponent(out crop)) {
                crop.DisableNeedingWater();
            }
        }
    }
    void Start() {
        AlertCrops();
    }
    public void Inform(Crop crop)
    {
        if ((crop.transform.position - transform.position).magnitude <= radius) {
            crop.DisableNeedingWater();
        }
    }
}
