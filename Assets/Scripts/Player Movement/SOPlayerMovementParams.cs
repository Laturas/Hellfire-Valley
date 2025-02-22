using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerMovementParams", menuName = "Scriptable Objects/SOPlayerMovementParams")]
public class SOPlayerMovementParams : ScriptableObject
{
    [Header("Camera")]
    public float Xsensitivity;
    public float Ysensitivity;
}
