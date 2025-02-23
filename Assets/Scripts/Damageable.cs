using UnityEngine;

public enum Team {
    FriendlyTeam,
    EnemyTeam,
}

public class Damageable : MonoBehaviour
{
    [SerializeField] private int defaultHealth;
    private int health;
    [SerializeField] public Team team {get; private set;}

    public void Start()
    {
        health = defaultHealth;
    }

    public void DealDamage(int amount, Team attackerTeam) {
        if (attackerTeam == team) {return;}
        health -= amount;
        if (health <= 0) {
            GameControl.instance.Die(DeathType.BuildingDeath, gameObject);
        }
    }
}
