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
    public float stopProjectionTimer = 0.1f;
    public float coyoteTimer = 0.3f;

    public float wallFloorBarrierAngle = 60f;

    [Header("Interaction")]
    public float placeDistance;
    public float placeCheckRadius;
    [Header("Water Hose")]
    public float hoseSprayOrigin;
    public float hoseSprayRadius;

    [Header("Buildings")]
    public float steepestBuildingPlaceAngle;
}
