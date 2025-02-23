using UnityEngine;

[CreateAssetMenu(fileName = "SOEnemy", menuName = "Scriptable Objects/SOEnemy")]
public class SOEnemy : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
}
