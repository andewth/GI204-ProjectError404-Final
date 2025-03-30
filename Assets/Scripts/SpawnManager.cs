using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public List<GameObject> enemyPrefabs;
    public GameObject powerUpPrefab;

    [System.Serializable]
    public class WaveData
    {
        public int totalSpawnEnemies;
        public int numberOfRandomSpawnPoint;
        public int delayStart;
        public float spawnInterval;
        public int numberOfPowerUp;
    }

    public List<WaveData> waves = new List<WaveData>();

    private List<Transform> selectedSpawnPoints;

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
        List<Transform> availableSpawnPoints = new List<Transform>(selectedSpawnPoints);

        while (enemiesSpawned < wave.totalSpawnEnemies)
        {
            if (availableSpawnPoints.Count == 0)
            {
                availableSpawnPoints = new List<Transform>(selectedSpawnPoints);
            }

            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[spawnIndex];
            availableSpawnPoints.RemoveAt(spawnIndex);

            SpawnEnemy(spawnPoint);
            enemiesSpawned++;

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnPowerUps(int numberOfPowerUp)
    {
        HashSet<int> usedIndexes = new HashSet<int>();

        for (int i = 0; i < numberOfPowerUp; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (!usedIndexes.Add(randomIndex));

            Instantiate(powerUpPrefab, spawnPoints[randomIndex].position, Quaternion.identity);
        }
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
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        if (enemyPrefabs.Count == 0) return;

        int enemyIndex = Random.Range(0, enemyPrefabs.Count);
        Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, Quaternion.identity);
    }
}
