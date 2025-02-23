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
    [SerializeField] private int startingPlayerMoney;
    [SerializeField] private int startingPlayerHealth;
    [field: SerializeField] public int playerHealth {get; private set;}
    [field: SerializeField] public int playerMoney {get; private set;}
    private int netWorth;
    [Header("Spawn Rate Curve")]
    [field: SerializeField] public float spawnRate {get; private set;}
    [SerializeField] private float spawnRateRiseFactor = 25f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        instance = this;
        DontDestroyOnLoad(this);

        playerMoney = startingPlayerMoney;
        netWorth = startingPlayerMoney;
        playerHealth = startingPlayerHealth;

        CalculateSpawnRate();
    }
    private bool paused = false;
    public GameObject pauseScreen;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
            if (paused) {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
            } else {
                pauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }
        CalculateSpawnRate();
    }

    public bool UpdateMoney(int changeAmount) {
        if (changeAmount >= 0) {
            netWorth += changeAmount;
        }
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

    // e^-(sqrt(x)/s)
    private void CalculateSpawnRate() {
        float exponent = -Mathf.Sqrt(netWorth) / spawnRateRiseFactor;
        spawnRate = 1 / Mathf.Exp(exponent);
    }
}
