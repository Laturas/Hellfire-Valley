using System;
using UnityEngine;
using UnityEngine.UI;

public enum DeathType {
    BuildingDeath,
    EnemyDeath,
    EnemyDeathApparition
}

[DefaultExecutionOrder(-1001)]
public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    public Transform playerTransform;
    public Camera playerCamera;
    public GameObject deadPlayer;
    public GameObject ui;
    [SerializeField] private int startingPlayerMoney;
    [SerializeField] private int startingPlayerHealth;
    [field: SerializeField] public int playerHealth {get; private set;}
    [field: SerializeField] public int playerMoney {get; private set;}
    [SerializeField] private int netWorth;
    [Header("Spawn Rate Curve")]
    [field: SerializeField] public float spawnRate {get; private set;}
    [SerializeField] private float spawnRateRiseFactor = 25f;
    [SerializeField] int[] waveStarts;
    private bool playerIsDead = false;

    public event Action<bool> OnPaused;
    
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
    private bool pausingEnabled = true;
    public GameObject pauseScreen;
    public GameObject hotbar;
    [field: SerializeField] public int currentWave {get; private set;}
    [SerializeField] bool hurtPlayer = false;

    public void TriggerOnDeathUIUpdates() {
        hotbar.SetActive(false);
        pausingEnabled = false;
        TogglePause(); // Because of the logic this closes the pause screen (maybe not necessary)
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
        CalculateSpawnRate();
        if (hurtPlayer) {UpdateHealth(-10); hurtPlayer = false;}
    }

    public void TogglePause() {
        if (!pausingEnabled) {
            paused = false;
            OnPaused?.Invoke(paused);
            return;
        }
        paused = !paused;
        if (paused) {
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        } else {
            pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
        OnPaused?.Invoke(paused);
    }

    public Slider sensitivitySlider;
    public void UpdateSensitivity() {
        GlobalSettings.instance.sensitivityModifier = sensitivitySlider.value;
    }
    public Slider volumeSlider;
    public void UpdateVolume() {
        GlobalSettings.instance.volumeModifier = volumeSlider.value;
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
        if (playerIsDead) {return;}
        playerHealth += changeAmount;
        if (playerHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer() {
        playerIsDead = true;
        deadPlayer.SetActive(true);
        deadPlayer.transform.position = playerTransform.position + new Vector3(0f,0.5f,0f);
        deadPlayer.transform.rotation = playerCamera.transform.rotation;
        playerCamera.gameObject.SetActive(false);
        playerTransform.gameObject.SetActive(false);
    }

    public void Die(DeathType deathType, GameObject dyingGameObject) {
        switch (deathType) {
            case DeathType.EnemyDeathApparition:
            Instantiate(SOManager.instance.enemyTypes[0].prefab, dyingGameObject.transform.position, Quaternion.identity); 
            Destroy(dyingGameObject); 
            break;
            case DeathType.BuildingDeath: 
            Destroy(dyingGameObject); 
            break;
            case DeathType.EnemyDeath: 
            Destroy(dyingGameObject); 
            break;
        }
    }

    // e^-(sqrt(x)/s)
    private void CalculateSpawnRate() {
        float exponent = -Mathf.Sqrt(netWorth) / spawnRateRiseFactor;
        spawnRate = 1 / Mathf.Exp(exponent);
        if (currentWave <= waveStarts.Length - 1) {
            if (waveStarts[currentWave] < netWorth) {
                currentWave++;
            }
        }
    }

}
