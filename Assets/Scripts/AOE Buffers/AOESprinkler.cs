using UnityEngine;

public class AOESprinkler : MonoBehaviour, IAOEBuff
{
    [SerializeField] private float radius = 5f;
    public void Inform(Crop crop)
    {
        if ((crop.transform.position - transform.position).magnitude <= radius) {
            crop.DisableNeedingWater();
        }
    }
}
