using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "(Deprecated) SOEnemy", menuName = "Scriptable Objects/(Deprecated) SOEnemy")]
public class DeprecatedSOEnemy : ScriptableObject
{
    public int BaseEnemyHealth = 1;
    public int BaseEnemyDamage = 1;
    public int BaseEnemyDetectionRadius = 50;
    public float BaseEnemyAttackCooldown = 2F;
    public float BaseEnemySpeed = 3F;

}
