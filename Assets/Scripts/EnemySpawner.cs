using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SOEnemy enemy;
    [SerializeField] private float timer;
    [SerializeField] private int wave;
    private float timerMax => 10f / GameControl.instance.spawnRate;

    void FixedUpdate()
    {
        if (timer > 0 && wave <= GameControl.instance.currentWave) {
            timer -= Time.fixedDeltaTime;
        } else {
            timer = timerMax;
            Instantiate(enemy.prefab, transform.position, Quaternion.identity);
        }
    }
}
