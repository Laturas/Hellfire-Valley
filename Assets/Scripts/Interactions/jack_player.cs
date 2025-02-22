using UnityEngine;
using UnityEngine.AI;
using VHierarchy.Libs;

public class jack_player : MonoBehaviour
{
    public int health;
    public int coinCount;
    NavMeshAgent agent;
    private float agentRedirectCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinCount = 0;
        health = 5;
        agent = GetComponent<NavMeshAgent>();
        agentRedirectCooldown = 3F;
    }

    // Update is called once per frame
    void Update()
    {
        agentRedirectCooldown -= Time.deltaTime;
        if (agentRedirectCooldown <= 0)
        {
            changeAgentGoal();
            agentRedirectCooldown = 3F;
        }
    }

    private void changeAgentGoal()
    {
        agent.destination = new Vector3(this.transform.position.x + Random.Range(-10, 10),
            this.transform.position.y,
            this.transform.position.z + Random.Range(-10, 10));
    }

    public void hurt()
    {
        health--;
        if (health == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "coin")
        {
            collider.gameObject.Destroy();
            coinCount++;
        }
    }
}
