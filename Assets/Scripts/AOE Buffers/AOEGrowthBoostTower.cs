using UnityEngine;

public class AOEGrowthBoostTower : MonoBehaviour, IAOEBuff
{
    [SerializeField] private float radius = 8f;
    [SerializeField] private float growthModifier = 2f;

    public void Inform(Crop crop) {
        if ((crop.transform.position - transform.position).magnitude <= radius) {
            crop.SetGrowthModifier(growthModifier);
        }
    }
}
