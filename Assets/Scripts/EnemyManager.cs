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

    public ParticleSystem explosion;
    public Transform explosionLocation;
    
    //Enemy variables:
    public int basicEnemyHP;
    public int basicEnemyDMG;
    public int basicEnemyGoldValue;
    public int basicEnemySpeed;
    
    public int tankEnemyHP;
    public int tankEnemyDMG;
    public int tankEnemyGoldValue;
    public int tankEnemySpeed;
    
    public int quickEnemyHP;
    public int quickEnemyDMG;
    public int quickEnemyGoldValue;
    public int quickEnemySpeed;

    public static int missile1DMG;
    public static int missile2DMG;
    public static int missile3DMG;
    
    void Start()
    {
        basicEnemyHP = 1;
        tankEnemyHP = 1;
        quickEnemyHP = 1;

        //Set default values and set agent destination:
        if (this.gameObject.tag == "Enemy1")
        {
            basicEnemyHP = 50 + (5 * SpawnEnemy.wavesSpawned);
            basicEnemyDMG = 5;
            basicEnemyGoldValue = 25;
            basicEnemySpeed = 40;
        }
        if (this.gameObject.tag == "Enemy2")
        {
            tankEnemyHP = 100 + (10 * SpawnEnemy.wavesSpawned);
            tankEnemyDMG = 10;
            tankEnemyGoldValue = 40;
            tankEnemySpeed = 20;
        }
        if (this.gameObject.tag == "Enemy3")
        {
            quickEnemyHP = 25 + (3 * SpawnEnemy.wavesSpawned);
            quickEnemyDMG = 5;
            quickEnemyGoldValue = 20;
            quickEnemySpeed = 60;
        }
        
        agent.SetDestination(HQLocation.position);
    }
    
    void Update()
    {
        //Speed of each enemy increases over time:
        if (this.gameObject.tag == "Enemy1")
        {
            agent.speed = basicEnemySpeed + (SpawnEnemy.wavesSpawned * 5);
        }

        if (this.gameObject.tag == "Enemy2")
        {
            agent.speed = tankEnemySpeed + (SpawnEnemy.wavesSpawned * 5);
        }
        if (this.gameObject.tag == "Enemy3")
        {
            agent.speed = quickEnemySpeed + (SpawnEnemy.wavesSpawned * 5);
        }
        
        //Limit for the enemy speed:
        if (agent.speed > 250)
        {
            agent.speed = 250;
        }
        
        //If enemy dies, give the player gold and destroy the enemy:
        if (basicEnemyHP <= 0)
        {
            GameManager.playerGold = GameManager.playerGold + basicEnemyGoldValue;
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

        if (tankEnemyHP <= 0)
        {
            GameManager.playerGold = GameManager.playerGold + tankEnemyGoldValue;
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
        if (quickEnemyHP <= 0)
        {
            GameManager.playerGold = GameManager.playerGold + quickEnemyGoldValue;
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        //When colliding with the HQ, deduct player's HQ health:
        if (other.collider.tag == "HQ")
        {
            if (this.gameObject.tag == "Enemy1")
            {
                HQManager.hqHP = HQManager.hqHP - basicEnemyDMG;
            }

            if (this.gameObject.tag == "Enemy2")
            {
                HQManager.hqHP = HQManager.hqHP - tankEnemyDMG;
            }
            if (this.gameObject.tag == "Enemy3")
            {
                HQManager.hqHP = HQManager.hqHP - quickEnemyDMG;
            }
            Instantiate(explosion, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    //Making sure to really destroy the missile (uses "gameObject.tag" instead of "collider.tag"):
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Missile1")
        {
            if (this.gameObject.tag == "Enemy1")
            {
                basicEnemyHP = basicEnemyHP - missile1DMG;
            }

            if (this.gameObject.tag == "Enemy2")
            {
                tankEnemyHP = tankEnemyHP - missile1DMG;
            }
            if (this.gameObject.tag == "Enemy3")
            {
                quickEnemyHP = quickEnemyHP - missile1DMG;
            }
            
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "Missile2")
        {
            if (this.gameObject.tag == "Enemy1")
            {
                basicEnemyHP = basicEnemyHP - missile2DMG;
            }

            if (this.gameObject.tag == "Enemy2")
            {
                tankEnemyHP = tankEnemyHP - missile2DMG;
            }
            if (this.gameObject.tag == "Enemy3")
            {
                quickEnemyHP = quickEnemyHP - missile2DMG;
            }
            
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.tag == "Missile3")
        {
            if (this.gameObject.tag == "Enemy1")
            {
                basicEnemyHP = basicEnemyHP - missile3DMG;
            }

            if (this.gameObject.tag == "Enemy2")
            {
                tankEnemyHP = tankEnemyHP - missile3DMG;
            }
            if (this.gameObject.tag == "Enemy3")
            {
                quickEnemyHP = quickEnemyHP - missile3DMG;
            }
            
            Destroy(other.gameObject);
        }
    }
}
