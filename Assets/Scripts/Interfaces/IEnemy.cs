using UnityEngine;

public interface IEnemy
{
    int enemyHealth { get; set; }
    void DamageEnemy(int dmg);
    void Die();
}
