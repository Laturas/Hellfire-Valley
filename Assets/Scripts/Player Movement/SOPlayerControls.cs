using UnityEngine;

[CreateAssetMenu(fileName = "SOPlayerMovementParams", menuName = "Scriptable Objects/SOPlayerMovementParams")]
public class SOPlayerControls : ScriptableObject
{
    [Header("Camera")]
    public float Xsensitivity;
    public float Ysensitivity;

    [Header("Movement")]
    public float playerSpeed;
    public float playerAirSpeed;
    public float playerAcceleration;
    public float playerAirAcceleration;
    public float jumpPower;

    public float wallFloorBarrierAngle = 60f;
}
