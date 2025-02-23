using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SOEnemy enemy;
    [SerializeField] private float timer;
    private float timerMax => 10f / GameControl.instance.spawnRate;

    void FixedUpdate()
    {
        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
        } else {
            timer = timerMax;
            Instantiate(enemy.prefab, transform.position, Quaternion.identity);
        }
    }
}
