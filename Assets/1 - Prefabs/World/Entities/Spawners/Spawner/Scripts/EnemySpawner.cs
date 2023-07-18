using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public int maxToSpawn;
    private int curNumSpawned;
    public float spawnDelay;

    void Start()
    {
        curNumSpawned = 0;
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        // if (Time.time - timeSinceLastSpawned >= spawnDelay && curNumSpawned < maxToSpawn)
        // {
        //     timeSinceLastSpawned = Time.time;
        //     curNumSpawned++;
        //     var enemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        // }
    }
    
    IEnumerator SpawnEnemies() {
        while (curNumSpawned < maxToSpawn) {
            var enemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
            curNumSpawned++;
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}