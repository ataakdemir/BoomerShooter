using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    public float spawnRateIncreaseInterval = 10f;
    public float spawnTimeReduction = 0.2f;
    public float minSpawnLimit = 0.5f;

    public int maxEnemies = 10;
    private int currentEnemyCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(IncreaseSpawnRate());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnDelay);

                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                currentEnemyCount++;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator IncreaseSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRateIncreaseInterval);

            minSpawnTime = Mathf.Max(minSpawnTime - spawnTimeReduction, minSpawnLimit);
            maxSpawnTime = Mathf.Max(maxSpawnTime - spawnTimeReduction, minSpawnLimit);

            Debug.Log($"Spawn hızı arttı! Yeni süreler: min={minSpawnTime}, max={maxSpawnTime}");
        }
    }
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
