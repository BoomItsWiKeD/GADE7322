using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public Transform currentSpawnPoint;
    public int enemiesToSpawn;
    public int baseEnemyAmount;
    
    public static int wavesSpawned;
    public float enemySpawnDelay;
    public float enemyWaveDelay;

    void Start()
    {
        enemySpawnDelay = 1.5f;
        baseEnemyAmount = 5;
        SpawnWave();
    }
    
    void Update()
    {
        enemyWaveDelay -= Time.deltaTime;
        if (enemyWaveDelay <= 0)
        {
            SpawnWave();
        }
        
        
        enemySpawnDelay -= Time.deltaTime;
        
        if (enemiesToSpawn > 0 && enemySpawnDelay <= 0)
        {
            enemySpawnDelay = 1.5f;
            
            Instantiate(enemyPrefab, currentSpawnPoint.transform.position, Quaternion.identity);
            enemiesToSpawn = enemiesToSpawn - 1;
        }
    }
    
    public void SpawnWave()
    {
        enemyWaveDelay = 30f - wavesSpawned;

        if (enemyWaveDelay <= 0)
        {
            enemySpawnDelay = 1;
        }
        wavesSpawned++;
        
        //Random spawn for wave:
        int num = UnityEngine.Random.Range(0, 4);
        
        //Current wave spawn point:
        currentSpawnPoint = spawnPoints[num];

        //For difficulty increase:
        enemiesToSpawn = baseEnemyAmount + wavesSpawned;
    }
}
