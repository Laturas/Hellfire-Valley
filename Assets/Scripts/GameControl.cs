using System;
using UnityEngine;

public enum DeathType {
    BuildingDeath,
    EnemyDeath,
}

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public Transform playerTransform;
    public Camera playerCamera;
    public GameObject ui;
    [field: SerializeField] public int playerHealth {get; private set;}
    [field: SerializeField] public int playerMoney {get; private set;}
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public bool UpdateMoney(int changeAmount) {
        playerMoney += changeAmount;
        if (playerMoney >= 0) return true;
        playerMoney -= changeAmount;
        return false;

    }
    public void UpdateHealth(int changeAmount) {
        playerHealth += changeAmount;
        if (playerHealth <= 0)
        {
            Debug.LogWarning("PLAYER DIED!!!!");
        }
    }

    public void Die(DeathType deathType, GameObject dyingGameObject) {
        switch (deathType) {
            case DeathType.BuildingDeath: Destroy(dyingGameObject); break;
            case DeathType.EnemyDeath: Destroy(dyingGameObject); break;
        }
    }
}
