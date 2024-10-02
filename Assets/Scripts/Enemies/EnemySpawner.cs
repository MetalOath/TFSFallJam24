using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;      // The enemy prefab to spawn
    public Transform[] spawnPoints;     // Array of spawn points
    public float spawnInterval = 5f;    // Time interval between spawns

    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        // Spawn an enemy at regular intervals
        if (_timer >= spawnInterval)
        {
            SpawnEnemy();
            _timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        // Choose a random spawn point
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        // Instantiate the enemy at the spawn point
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
