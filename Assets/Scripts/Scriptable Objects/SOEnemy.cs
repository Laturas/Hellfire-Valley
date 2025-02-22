using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "SOEnemy", menuName = "Scriptable Objects/SOEnemy")]
public class SOEnemy : ScriptableObject
{
    public int BaseEnemyHealth = 1;
    public int BaseEnemyDamage = 1;
    public int BaseEnemyDetectionRadius = 50;
    public float BaseEnemyAttackCooldown = 2F;
    public float BaseEnemySpeed = 3F;

}
