using System.Runtime.InteropServices;
using UnityEngine;

public enum Team {
    FriendlyTeam,
    EnemyTeam,
}

public class Damageable : MonoBehaviour
{
    [SerializeField] private int defaultHealth;
    [SerializeField] private bool thisIsPlayer;
    private int health;
    [field: SerializeField] public Team team {get; private set;}

    public void Start()
    {
        health = defaultHealth;
    }

    public void DealDamage(int amount, Team attackerTeam) {
        if (attackerTeam == team) {return;}
        if (thisIsPlayer) {
            GameControl.instance.UpdateHealth(-amount);
            return;
        }
        health -= amount;
        Debug.Log(team + "" + health);
        if (health <= 0) {
            GameControl.instance.Die(DeathType.BuildingDeath, gameObject);
        }
    }
}
