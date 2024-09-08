using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform HQLocation;
    
    public int enemyHP;
    public int enemyDMG;
    public int enemyGoldValue;
    void Start()
    {
        enemyHP = 50;
        enemyDMG = 5;
        enemyGoldValue = 25;
        agent.SetDestination(HQLocation.position);
    }
    
    void Update()
    {
        if (enemyHP <= 0)
        {
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
}
