using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int hp = 1;
    public float attackRange = 2;
    public int attackDamage = 1;
    private GameObject player;
    private NavMeshAgent navMeshAgent;
    private EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Set stuff
        enemyState = EnemyState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the enemy
        UpdateStateEnemy();
    }

    void UpdateStateEnemy()
    {
        // change this to bs fsm ...
        switch (enemyState)
        {
            case EnemyState.idle:
                // Find player
                if (PlayerIsAlive())
                {
                    enemyState = EnemyState.chasing;
                }
                break;
            case EnemyState.chasing:
                // Is there still a player?
                if (PlayerIsAlive() == false)
                {
                    enemyState = EnemyState.idle;
                    break;
                }

                // Chase player
                EnemyPathfinding();

                // Check if in attacking distance
                if(EnemyCanAttack(attackRange))
                {
                    enemyState = EnemyState.attacking;
                    break;
                }
                break;
            case EnemyState.attacking:
                // Attack player
                AttackPlayer();

                // Go back to idle
                enemyState = EnemyState.idle;

                break;
            case EnemyState.dying:
                // Enemy dies
                Destroy(gameObject);
                break;
            default: Debug.LogError("Error::Enemy::R = Enemy state isnt defined!");
                break;
        }
    }

    /// <summary>
    /// Enemy recieves damage
    /// </summary>
    /// <param name="dmg">Amount of damage the enemy recieves</param>
    public void RecieveDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 1)
        {
            // Dead
            enemyState = EnemyState.dying;
        }
    }

    private void EnemyPathfinding()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }

    /// <summary>
    /// Check if player is "alive" (is there a player gameobject?)
    /// </summary>
    /// <returns></returns>
    bool PlayerIsAlive()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Get the distance to the player
    /// </summary>
    /// <returns>Returns the distance between this and the player</returns>
    float DistanceToPlayer()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        return Vector3.Distance(player.position, transform.position);
    }

    bool EnemyCanAttack(float range)
    {
        if(DistanceToPlayer() < range)
        {
            return true;
        }
        return false;
    }

    private void AttackPlayer()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().PlayerRecieveDamage(attackDamage);
    }
}

public enum EnemyState
{
    idle,
    chasing,
    attacking,
    dying
}