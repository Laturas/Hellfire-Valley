using UnityEngine;
using UnityEngine.AI;
using VHierarchy.Libs;

public class PlayerEnemyTarget : MonoBehaviour
{
    public int coinCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hurt()
    {
        GameControl.instance.UpdateHealth(-1);
        if (GameControl.instance.playerHealth == 0)
        {
            Time.timeScale = 0;
            Debug.Log("Player died!");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
    }
}
