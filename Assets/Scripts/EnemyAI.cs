using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Melee, Ranged }
public class EnemyAI : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.Melee;

    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    private bool canDealDamage = false;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Ranged Enemy Settings")]
    public Transform shootPoint;

    private Animator animator;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        animator.SetBool("isAttacking", playerInAttackRange);

        if (!playerInSightRange && !playerInAttackRange)
            Patrolling();
        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        else if (playerInSightRange && playerInAttackRange)
        {
            if (enemyType == EnemyType.Melee)
                AttackPlayer();
            else if (enemyType == EnemyType.Ranged)
                RangedAttack();
        }
    }
    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            animator.ResetTrigger("Attack");
            canDealDamage = false;
            ResetAttack();
            return;
        }
        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (!alreadyAttacked)
        {
            canDealDamage = true;
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void RangedAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            animator.ResetTrigger("Attack");
            canDealDamage = false;
            ResetAttack();
            return;
        }
        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 direction = (player.position + Vector3.up * 1f - shootPoint.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);

        if (!alreadyAttacked)
        {
            canDealDamage = true;            
            animator.SetTrigger("Attack");    
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void DealDamage()
    {
        if (!canDealDamage) return;
        canDealDamage = false; 

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange) return;

        Movement pm = player.GetComponent<Movement>();
        if (pm != null)
            pm.PlayerTakesDamage(20f);
    }
    public void ShootRaycast()
    {
        if (!canDealDamage) return;
        canDealDamage = false;

        // Ray menzili olarak attackRange kullanılıyor
        RaycastHit hit;
        Vector3 origin = shootPoint.position;
        Vector3 direction = (player.position + Vector3.up * 1f - origin).normalized;

        if (Physics.Raycast(origin, direction, out hit, attackRange))
        {
            // İsteğe bağlı: Debug çizgisi görebilmek için
            Debug.DrawLine(origin, hit.point, Color.red, 1f);

            if (hit.collider.CompareTag("Player"))
            {
                var pm = hit.collider.GetComponent<Movement>();
                if (pm != null)
                    pm.PlayerTakesDamage(20f);
            }
        }
        else
        {
            // Ray hiçbir şey bulamadıysa
            Debug.DrawRay(origin, direction * attackRange, Color.yellow, 1f);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        canDealDamage = false;
        agent.isStopped = false;
        agent.updateRotation = true;
    }




    private void OnDrawGizmosSelected()
    {
        if (shootPoint == null) return;

        // Başlangıç
        Vector3 origin = shootPoint.position;
        // Hedefe yön
        Vector3 dir = (player != null)
            ? (player.position + Vector3.up * 1f - origin).normalized
            : transform.forward;
        // Kırmızı çizgi
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + dir * attackRange);
    }
}
