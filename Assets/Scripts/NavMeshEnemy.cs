using UnityEngine;
using UnityEngine.AI;

public class NavMeshEnemy : MonoBehaviour
{
    private Damageable target;
    public bool isTouching;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int baseMeleeDamage = 5;
    private float currentCooldown;
    public float agentRedirectCooldown;
    private NavMeshAgent agent;
    private float overlapSphereCooldown = 0.25f;
    [SerializeField] private float awarenessRadius = 10f;
    public Animator enemyAnimator;
    private bool isIdle;
    private float idleUpdateCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // navmesh setup
        agent = GetComponent<NavMeshAgent>();
        agent.destination = GameControl.instance.playerTransform.position;
        agentRedirectCooldown = 1.5F;

        //idle movement setup
        idleUpdateCooldown = 5F;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAnimator != null) {
            if (agent.remainingDistance < 1f) {
                enemyAnimator.SetBool("Running", false);
            } else {
                enemyAnimator.SetBool("Running", true);
            }
        }
        updateAttackCooldowns();

        overlapSphereCooldown -= Time.deltaTime;
        if (overlapSphereCooldown <= 0)
        {
            updatePathAndTarget();
        }
    }

    private void updateAttackCooldowns()
    {
        currentCooldown -= (currentCooldown <= 0) ? 0 : Time.deltaTime;
        if (isTouching) {
            Attack(target);
        }
    }
    private void Attack(Damageable attackTarget) {
        if (currentCooldown > 0) {
            return;
        }
        currentCooldown = attackCooldown;
        attackTarget.DealDamage(baseMeleeDamage, Team.EnemyTeam);
    }

    private void updatePathAndTarget()
    {
        Collider[] colliderResult = new Collider[10];
        // idea: small chance to move target position to a tower if it is damaged by it?

        int count = Physics.OverlapSphereNonAlloc(transform.position, awarenessRadius, colliderResult, 1 << 11 | 1 << 12, QueryTriggerInteraction.Collide);
        Damageable closestTarget = null;
        Damageable potentialTarget;
        for (int i = 0; i < count; i++)
        {
            Collider targetCandidate = colliderResult[i];
            if (targetCandidate.TryGetComponent(out potentialTarget)) {
                if (potentialTarget.team == Team.EnemyTeam) {continue;}
                if (closestTarget == null)
                {
                    closestTarget = potentialTarget;
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, closestTarget.transform.position) >
                        Vector3.Distance(this.transform.position, target.gameObject.transform.position))
                    {
                        closestTarget = potentialTarget;
                    }
                }
            }
        }
        changeAgentGoal((closestTarget == null) ? null : closestTarget.gameObject);
    }
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position, awarenessRadius);
    // }

    private void changeAgentGoal(GameObject obj)
    {
        if (obj != null)
        {
            agent.destination = obj.transform.position;
            isIdle = false;
        }
        else
        {
            if (!isIdle || idleUpdateCooldown <= 0)
            {
                agent.destination = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y, transform.position.z + Random.Range(-10, 10));
                idleUpdateCooldown = 5f;
            }
            idleUpdateCooldown -= Time.deltaTime;
            isIdle = true;
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Attack");
        Damageable maybeDamage;
        if (collision.gameObject.TryGetComponent(out maybeDamage)) {
            target = maybeDamage;
            enemyAnimator.SetTrigger("Attack");
            maybeDamage.DealDamage(baseMeleeDamage, Team.EnemyTeam);
            isTouching = true;
        }
    }

    // void OnTriggerExit(Collider collision)
    // {
    //     Damageable maybeDamage;
    //     if (collision.gameObject.TryGetComponent(out maybeDamage)) {
    //         target = maybeDamage;
    //         maybeDamage.DealDamage(baseMeleeDamage, Team.EnemyTeam);
    //         isTouching = false;
    //     }

    // }
}


// using UnityEngine;
// using UnityEngine.AI;

// public class NavMeshEnemy : MonoBehaviour 
// {
//     [Header("Parameters")]
//     [SerializeField] private float awarenessRadius;
//     [SerializeField] private float damageAmount;
//     [SerializeField] private float attackCooldown;

//     private float currentCooldown;
//     private Damageable target;
//     private Damageable thisDamageable;
//     private NavMeshAgent agent;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         thisDamageable = GetComponent<Damageable>();
//         agent = GetComponent<NavMeshAgent>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         target ??= TryLookForTarget();
//         if (target != null) {
//             agent.SetDestination(target.transform.position);
//         }
//     }

//     private Damageable TryLookForTarget() {
//         Collider[] colliderResult = new Collider[10];
//         int count = Physics.OverlapSphereNonAlloc(transform.position, awarenessRadius, colliderResult, 1 << 11 | 1 << 12, QueryTriggerInteraction.Collide);
//         Damageable potentialTarget;
//         for (int i = 0; i < count; i++) {
//             if (colliderResult[i].TryGetComponent(out potentialTarget)) {
//                 if (potentialTarget.team == thisDamageable.team) {
//                     return potentialTarget;
//                 }
//             }
//         }
//         return null;
//     }
// }
