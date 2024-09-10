using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class EnemyManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform HQLocation;
    
    public int enemyHP;
    public int enemyDMG;
    public int enemyGoldValue;
    void Start()
    {
        enemyHP = 50 + (5 * SpawnEnemy.wavesSpawned);
        enemyDMG = 5;
        enemyGoldValue = 25;
        agent.SetDestination(HQLocation.position);
    }
    
    void Update()
    {
        agent.speed = 40 + (SpawnEnemy.wavesSpawned * 5);

        if (agent.speed > 250)
        {
            agent.speed = 250;
        }
        
        if (enemyHP <= 0)
        {
            GameManager.playerGold = GameManager.playerGold + 30;
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "HQ")
        {
            HQManager.hqHP = HQManager.hqHP - enemyDMG;
            Debug.Log("" +HQManager.hqHP);
            Destroy(this.gameObject);
        }

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
