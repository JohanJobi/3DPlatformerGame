using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy1 : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform player2;
    public GroundItem objectToDropPrefab;
    public ItemDatabaseObject database;
    public LayerMask Ground, Player, Player2;
    
    public int health = 100;
    //patrolling

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool player2InSightRange, player2InAttackRange;

    public healthBar healthBar;

    private void Awake()
    {
        player = GameObject.Find("player1").transform;
        player2 = GameObject.Find("player2").transform;
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<healthBar>();
        healthBar.setMaxHealth(health);

    }


    private void Update()
    {
        
        //check for sight and attack range of both players
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, Player);
        player2InSightRange = Physics.CheckSphere(transform.position, sightRange, Player2);
        player2InAttackRange = Physics.CheckSphere(transform.position, attackRange, Player2);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if(!player2InSightRange && !player2InAttackRange) Patrolling();

        if (playerInSightRange && !playerInAttackRange) ChasePlayer(player);
        else if (player2InSightRange && !player2InAttackRange) ChasePlayer(player2);

        if (playerInAttackRange && playerInSightRange) AttackPlayer(player);
        else if (player2InAttackRange && player2InSightRange) AttackPlayer(player2); 
        }
    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculates random point in range
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer(Transform focusPlayer)
    {
        agent.SetDestination(focusPlayer.position);
    }

    private void AttackPlayer(Transform focusPlayer)
    {
        //make sure enemy doesnt move

        agent.SetDestination(transform.position);

        transform.LookAt(focusPlayer);

        if (!alreadyAttacked)
        {
            //Attack Code
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.setHealth(health);
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {

        Destroy(gameObject);
        objectToDropPrefab.item.data.Id = 0;
        Instantiate(objectToDropPrefab, gameObject.transform.position, Quaternion.identity);

        var RandomId = UnityEngine.Random.Range(0, 8);
        ItemObject item = database.GetItem[RandomId];
        objectToDropPrefab.item = item;
        Instantiate(objectToDropPrefab, gameObject.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        //visual representation of the attack and sight range of the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
