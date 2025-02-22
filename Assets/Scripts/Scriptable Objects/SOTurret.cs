using UnityEngine;

[CreateAssetMenu(fileName = "SOTurret", menuName = "Scriptable Objects/SOTurret")]
public class SOTurret : ScriptableObject
{
    public GameObject prefab;
    [Header("Tracking")]
    public float rotateSpeed;
    [Header("Attacking")]
    public float fireDelay;
    public float range;
}
