using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public int maxToSpawn;
    private int curNumSpawned;
    public float spawnDelay;
    private float timeSinceLastSpawned;

    void Start()
    {
        curNumSpawned = 0;
        timeSinceLastSpawned = Time.time;
    }

    void Update()
    {
        if (Time.time - timeSinceLastSpawned >= spawnDelay && curNumSpawned < maxToSpawn)
        {
            timeSinceLastSpawned = Time.time;
            curNumSpawned++;
            var enemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
            enemy.SetActive(true);
        }
    }
}
