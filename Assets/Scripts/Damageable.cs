using UnityEngine;

public enum Team {
    FriendlyTeam,
    EnemyTeam,
}

public class Damageable : MonoBehaviour
{
    [SerializeField] private int defaultHealth;
    private int health;
    [SerializeField] public Team thisTeam;

    public void Start()
    {
        health = defaultHealth;
    }

    public void DealDamage(int amount, Team attackerTeam) {
        if (attackerTeam == thisTeam) {return;}
        health -= amount;
        if (health <= 0) {
            // Die
        }
    }
}
