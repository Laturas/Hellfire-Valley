using UnityEngine;

public class AOEGrowthBoostTower : MonoBehaviour, IAOEBuff
{
    [SerializeField] private float radius = 8f;
    [SerializeField] private float growthModifier = 2f;

    public void AlertCrops()
    {
        Collider[] crops = Physics.OverlapSphere(transform.position, radius, 1 << 13, QueryTriggerInteraction.Collide);

        foreach (Collider cropCollider in crops) {
            Crop crop;
            if (cropCollider.TryGetComponent(out crop)) {
                crop.SetGrowthModifier(2f);
            }
        }
    }
    void Start()
    {
        AlertCrops();   
    }

    public void Inform(Crop crop) {
        if ((crop.transform.position - transform.position).magnitude <= radius) {
            crop.SetGrowthModifier(growthModifier);
        }
    }
}
