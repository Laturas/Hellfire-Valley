using System.Runtime.InteropServices;
using UnityEngine;

public enum Team {
    FriendlyTeam,
    EnemyTeam,
}

public enum Variant {
    Vanilla,
    Apparition
}

public class Damageable : MonoBehaviour
{
    [SerializeField] private int defaultHealth;
    [SerializeField] private bool thisIsPlayer;
    private int health;
    [field: SerializeField] public Team team {get; private set;}
    [field: SerializeField] public Variant variant {get; private set;}

    public void Start()
    {
        health = defaultHealth;
    }

    public void DealDamage(int amount, Team attackerTeam) {
        if (attackerTeam == team) {return;}
        if (thisIsPlayer) {
            GameControl.instance.UpdateHealth(-amount);
            Debug.Log(team + "" + health);
            return;
        }
        health -= amount;
        Debug.Log(team + "" + health);
        if (health <= 0) {
            if (variant == Variant.Apparition) {
                GameControl.instance.Die(DeathType.EnemyDeathApparition, gameObject);
            }
            else if (team == Team.FriendlyTeam) {
                GameControl.instance.Die(DeathType.BuildingDeath, gameObject);
            }
            else if (team == Team.EnemyTeam) {
                GameControl.instance.Die(DeathType.EnemyDeath, gameObject);
            }
        }
    }
}
