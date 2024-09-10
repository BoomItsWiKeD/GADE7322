using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyManager : MonoBehaviour
{
    //NavMeshAgent variables:
    public NavMeshAgent agent;
    public Transform HQLocation;
    
    //Enemy variables:
    public int enemyHP;
    public int enemyDMG;
    public int enemyGoldValue;
    
    void Start()
    {
        //Set default values and set agent destination:
        enemyHP = 50 + (5 * SpawnEnemy.wavesSpawned);
        enemyDMG = 5;
        enemyGoldValue = 25;
        
        agent.SetDestination(HQLocation.position);
    }
    
    void Update()
    {
        //Speed of each enemy increases over time:
        agent.speed = 40 + (SpawnEnemy.wavesSpawned * 5);

        //Limit for the enemy speed:
        if (agent.speed > 250)
        {
            agent.speed = 250;
        }
        
        //If enemy dies, give the player gold and destroy the enemy:
        if (enemyHP <= 0)
        {
            GameManager.playerGold = GameManager.playerGold + 30;
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //When colliding with the HQ, deduct player's HQ health:
        if (other.collider.tag == "HQ")
        {
            HQManager.hqHP = HQManager.hqHP - enemyDMG;
            Destroy(this.gameObject);
        }

        //When colliding with a tower's missile, deduct enemy HP and destroy the missile:
        if (other.collider.tag == "Missile")
        {
            enemyHP = enemyHP - 25;
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Missile")
        {
            enemyHP = enemyHP - 25;
            Destroy(other.gameObject);
            
        }
    }
}
