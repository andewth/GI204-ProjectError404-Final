using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public List<GameObject> enemyPrefabs; // List of enemy prefabs
    public GameObject powerUpPrefab; // Prefab ของ PowerUp

    [System.Serializable]
    public class WaveData
    {
        public int totalSpawnEnemies;
        public int numberOfRandomSpawnPoint;
        public int delayStart;
        public float spawnInterval;
        public int numberOfPowerUp;
    }

    public List<WaveData> waves = new List<WaveData>(); // Wave Array

    private List<Transform> selectedSpawnPoints; // Keep Spawn Array

    void Start()
    {
        StartCoroutine(WaveManagerRoutine());
    }

    IEnumerator WaveManagerRoutine()
    {
        for (int waveIndex = 0; waveIndex < waves.Count; waveIndex++)
        {
            WaveData currentWave = waves[waveIndex];

            Debug.Log($"Starting Wave #{waveIndex + 1}");

            yield return new WaitForSeconds(currentWave.delayStart);

            SpawnPowerUps(currentWave.numberOfPowerUp);

            SelectRandomSpawnPoints(currentWave.numberOfRandomSpawnPoint);

            yield return StartCoroutine(SpawnEnemyRoutine(currentWave));

            Debug.Log($"Wave #{waveIndex + 1} Completed!");
        }

        Debug.Log("All Waves Completed!");
    }

    IEnumerator SpawnEnemyRoutine(WaveData wave)
    {
        int enemiesSpawned = 0;

        while (enemiesSpawned < wave.totalSpawnEnemies)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnPowerUps(int numberOfPowerUp)
    {
        for (int i = 0; i < numberOfPowerUp; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(
                powerUpPrefab,
                spawnPoints[randomIndex].position,
                Quaternion.identity
            );
        }

        Debug.Log($"Spawned {numberOfPowerUp} PowerUps at random locations.");
    }

    void SelectRandomSpawnPoints(int numberOfRandomSpawnPoint)
    {
        selectedSpawnPoints = new List<Transform>();

        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableIndexes.Add(i);
        }

        for (int i = 0; i < numberOfRandomSpawnPoint && availableIndexes.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableIndexes.Count);
            selectedSpawnPoints.Add(spawnPoints[availableIndexes[randomIndex]]);
            availableIndexes.RemoveAt(randomIndex);
        }

        Debug.Log("Selected Spawn Points: " + string.Join(", ", selectedSpawnPoints));
    }

    void SpawnEnemy()
    {
        if (selectedSpawnPoints.Count == 0 || enemyPrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, selectedSpawnPoints.Count);
        int enemyIndex = Random.Range(0, enemyPrefabs.Count); // Randomly select an enemy

        Instantiate(
            enemyPrefabs[enemyIndex], // Spawn a random enemy from the list
            selectedSpawnPoints[randomIndex].position,
            Quaternion.identity
        );
    }
}
