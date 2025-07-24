using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;         // Assign your prefab here
    public Transform[] spawnPoints;        // Assign spawn locations
    public float spawnDelay = 2f;          // Time between spawns
    public int enemyCount = 5;             // Number of enemies to spawn

    private int spawned = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnDelay);
    }

    void SpawnEnemy()
    {
        if (spawned >= enemyCount)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawned++;
    }
}
