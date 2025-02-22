using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public jack_player playerScript;
    public tower_jack towerScript;
    public GameObject player;
    public GameObject tower;
    public bool isTouchingTower;
    public bool isTouchingPlayer;
    public float attackMaxCooldown;
    private bool attackTowerIsOnCooldown;
    private bool attackPlayerIsOnCooldown;
    private float attackTowerCountdown;
    private float attackPlayerCountdown;
    private float agentRedirectCooldown;
    private NavMeshAgent agent;
    public SOEnemy SOEnemy;
    private float overlapSphereCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        attackMaxCooldown = 2F;
        attackTowerCountdown = attackMaxCooldown;
        attackPlayerCountdown = attackMaxCooldown;

        agentRedirectCooldown = 1.5F;

        overlapSphereCooldown = 0.25F;
    }

    // Update is called once per frame
    void Update()
    {
        agentRedirectCooldown -= Time.deltaTime;
        if (agentRedirectCooldown <= 0)
        {
            changeAgentGoal(player);
            agentRedirectCooldown = 0.6F;
        }

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
                playerScript.hurt();
            }
        }

        overlapSphereCooldown -= Time.deltaTime;
        if (overlapSphereCooldown <= 0)
        {
            Collider[] detectedTargets = Physics.OverlapSphere(transform.position, SOEnemy.BaseEnemyDetectionRadius);
        }
    }

    private void resetAttackTimer(float timer)
    {
        timer = attackMaxCooldown;
    }

    private void changeAgentGoal(GameObject obj)
    {
        agent.destination = obj.transform.position;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!attackPlayerIsOnCooldown)
            {
                playerScript.hurt();
                resetAttackTimer(attackPlayerCountdown);
            }
            attackPlayerIsOnCooldown = true;
            isTouchingPlayer = true;
        }
        else if (collision.gameObject.tag == "tower")
        {
            isTouchingTower = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isTouchingPlayer = false;
        isTouchingTower = false;
    }
}
