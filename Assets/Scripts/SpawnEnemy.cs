using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //Spawn positions for the enemies and transforms to store (x, y and z) positions:
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;
    public Transform currentSpawnPoint;
    
    //Number of enemies variables:
    public int enemiesToSpawn;
    public int baseEnemyAmount;
    
    //Enemy wave-related variables:
    public static int wavesSpawned;
    public float enemySpawnDelay;
    public float enemyWaveDelay;

    void Start()
    {
        //Set variables to default values:
        wavesSpawned = 0;
        enemySpawnDelay = 1.5f;
        baseEnemyAmount = 5;
        
        //And spawn the first wave:
        SpawnWave();
    }
    
    void Update()
    {
        //Have a countdown for the next enemy wave incoming:
        enemyWaveDelay -= Time.deltaTime;
        
        //When timer reaches 0, spawn a new wave:
        if (enemyWaveDelay <= 0)
        {
            SpawnWave();
        }
        
        //Countdown for each enemy per wave:
        enemySpawnDelay -= Time.deltaTime;
        
        //Instantiate enemy prefab on the map:
        if (enemiesToSpawn > 0 && enemySpawnDelay <= 0)
        {
            enemySpawnDelay = 1.5f;
            
            int randomEnemy = UnityEngine.Random.Range(0, 3);
            
            Instantiate(enemyPrefab[randomEnemy], currentSpawnPoint.transform.position, Quaternion.identity);
            enemiesToSpawn = enemiesToSpawn - 1;
        }
    }
    
    //Handles spawning of the enemies and the enemy wave cooldown:
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

        if (num == 0) //Top spawn
        {
            //Random spawn point to be moved along Z:
            int transformSpawnNum = Random.Range(-968, 1251);

            currentSpawnPoint.position =
                new Vector3(currentSpawnPoint.position.x, currentSpawnPoint.position.y, transformSpawnNum);

        }
        if (num == 1) //Left spawn
        {
            //Random spawn point to be moved along X:
            int transformSpawnNum = Random.Range(-1268, 938);
            
            currentSpawnPoint.position =
                new Vector3(transformSpawnNum, currentSpawnPoint.position.y, currentSpawnPoint.position.z);
        }
        if (num == 2) //Right spawn
        {
            //Random spawn point to be moved along X:
            int transformSpawnNum = Random.Range(-1243, 967);
            
            currentSpawnPoint.position =
                new Vector3(transformSpawnNum, currentSpawnPoint.position.y, currentSpawnPoint.position.z);
        }
        if (num == 3) //Bottom spawn
        {
            //Random spawn point to be moved along Z:
            int transformSpawnNum = Random.Range(-943, 1235);
            
            currentSpawnPoint.position =
                new Vector3(currentSpawnPoint.position.x, currentSpawnPoint.position.y, transformSpawnNum);
        }

        //For difficulty increase:
        enemiesToSpawn = baseEnemyAmount + wavesSpawned;
    }
}
