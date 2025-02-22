using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private PlayerEnemyTarget playerScript;
    public tower_jack towerScript;
    private GameObject player;
    public GameObject tower;
    public bool isTouchingTower;
    public bool isTouchingPlayer;
    public float attackMaxCooldown;
    private bool attackTowerIsOnCooldown;
    private bool attackPlayerIsOnCooldown;
    private float attackTowerCountdown;
    private float attackPlayerCountdown;
    public float agentRedirectCooldown;
    private NavMeshAgent agent;
    public SOEnemy SOEnemy;
    private float overlapSphereCooldown;
    private int overlapSphereRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // navmesh setup
        agent = GetComponent<NavMeshAgent>();
        playerScript = FindFirstObjectByType<PlayerEnemyTarget>();
        player = playerScript.gameObject;
        agent.destination = GameControl.instance.playerTransform.position;
        agentRedirectCooldown = 1.5F;

        // collision and attack cooldown setup
        attackMaxCooldown = 2F;
        attackTowerCountdown = attackMaxCooldown;
        attackPlayerCountdown = attackMaxCooldown;

        // overlap sphere setup
        overlapSphereCooldown = 0.25F;
        overlapSphereRadius = SOEnemy.BaseEnemyDetectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        // agentRedirectCooldown -= Time.deltaTime;
        // if (agentRedirectCooldown <= 0)
        // {
        //     changeAgentGoal(player);
        //     agentRedirectCooldown = 0.6F;
        // }

        if (attackTowerIsOnCooldown)
        {
            attackTowerCountdown -= Time.deltaTime;
        }
        if (attackTowerCountdown <= 0)
        {
            attackTowerCountdown = attackMaxCooldown;
            if (!isTouchingTower)
            {
                attackTowerIsOnCooldown = false;
            }
            else
            {
                towerScript.hurt();
            }
        }
        if (attackPlayerIsOnCooldown)
        {
            attackPlayerCountdown -= Time.deltaTime;
        }
        if (attackPlayerCountdown <= 0)
        {
            attackPlayerCountdown = attackMaxCooldown;
            if (!isTouchingPlayer)
            {
                attackPlayerIsOnCooldown = false;
            }
            else
            {
                playerScript.Hurt();
            }
        }

        overlapSphereCooldown -= Time.deltaTime;
        if (overlapSphereCooldown <= 0)
        {
            // idea: small chance to move target position to a tower if it is damaged by it?

            Collider[] detectedTargets = Physics.OverlapSphere(transform.position, overlapSphereRadius);

            GameObject closestTarget = null;
            foreach (Collider target in detectedTargets)
            {
                if (target.gameObject == this || target.gameObject.layer == 10 || target.gameObject.tag == "Enemy")
                {
                    continue;
                }
                if (target.gameObject.tag == "Player")
                {
                    closestTarget = target.gameObject;
                    break;
                }
                if (closestTarget == null)
                {
                    closestTarget = target.gameObject;
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, closestTarget.transform.position) >
                        Vector3.Distance(this.transform.position, target.gameObject.transform.position))
                    {
                        closestTarget = target.gameObject;
                    }
                }
            }

            changeAgentGoal(closestTarget);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, overlapSphereRadius);
    }

    private void resetAttackTimer(float timer)
    {
        timer = attackMaxCooldown;
    }

    private void changeAgentGoal(GameObject obj)
    {
        if (obj != null)
        {
            agent.destination = new Vector3(obj.transform.position.x, 0.85F, obj.transform.position.z);
        }
        else
        {
            agent.destination = new Vector3(this.transform.position.x + Random.Range(-10, 10), 0.85F,
                this.transform.position.z + Random.Range(-10, 10));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!attackPlayerIsOnCooldown)
            {
                playerScript.Hurt();
                resetAttackTimer(attackPlayerCountdown);
            }
            attackPlayerIsOnCooldown = true;
            isTouchingPlayer = true;
        }
        else if (collision.gameObject.tag == "tower")
        {
            isTouchingTower = true;
            overlapSphereRadius = SOEnemy.BaseEnemyDetectionRadius / 5;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "player")
        {
            isTouchingPlayer = false;
        }
        else if (collision.gameObject.tag == "tower")
        {
            isTouchingTower = false;
            overlapSphereRadius = SOEnemy.BaseEnemyDetectionRadius;
        }

    }
}
