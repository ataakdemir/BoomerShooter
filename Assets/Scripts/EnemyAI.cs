using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType { Melee, Ranged}
public class EnemyAI : MonoBehaviour
{

    public EnemyType enemyType = EnemyType.Melee;


    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Ranged Enemy Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed;

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

        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        if (playerInSightRange && playerInAttackRange)
        {
            if (enemyType == EnemyType.Melee)
                AttackPlayer();
            else if (enemyType == EnemyType.Ranged)
                RangedAttack();
        }
            
    }
    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;   
    }

    private void SearchWalkPoint()  
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");

            Movement playerMovement = player.GetComponent<Movement>();
            if (playerMovement != null)
            {
                playerMovement.PlayerTakesDamage(20f); 
            } 

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    
    }
    private void RangedAttack()
    {
        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 direction = (player.position + Vector3.up * 1f - shootPoint.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

        }
    }
    public void ShootProjectile()
    {
        Vector3 direction = (player.position + Vector3.up * 1f - shootPoint.position).normalized;
        GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = direction * projectileSpeed;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        agent.isStopped = false;
        agent.updateRotation = true;
    }

}
