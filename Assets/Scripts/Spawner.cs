using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    public float spawnTimeReduction = 0.2f;
    public float minSpawnLimit = 0.5f;

    private int spawnedEnemiesCount = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnDelay);

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            spawnedEnemiesCount++;

            if (spawnedEnemiesCount % 5 == 0)
            {
                minSpawnTime = Mathf.Max(minSpawnTime - spawnTimeReduction, minSpawnLimit);
                maxSpawnTime = Mathf.Max(maxSpawnTime - spawnTimeReduction, minSpawnLimit);

                Debug.Log($"Her 5 düşmandan sonra spawn hızlandı! Yeni süreler: min={minSpawnTime}, max={maxSpawnTime}");
            }
        }
    }
}
