using UnityEngine;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array of enemy types
    public Transform[] spawnPoints;    // Spawn locations
    public float spawnDelay = 1f;      // Delay between spawns

    private int waveNumber = 1;        // Current wave number
    private int totalEnemiesInWave;    // Total enemies in current wave
    private int enemiesSpawned;        // Track how many have spawned
    private List<GameObject> activeEnemies = new List<GameObject>(); // Track alive enemies
    private float nextSpawnTime;       // Control spawn timing
    private bool waveInProgress = false; // Ensure waves do not overlap

    void Start()
    {
        StartWave();
    }

    void Update()
    {
        // ✅ SPAWN LOGIC: Spawn enemies one by one
        if (waveInProgress && enemiesSpawned < totalEnemiesInWave && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay;
        }

        // ✅ CHECK LIVE ENEMIES
        activeEnemies.RemoveAll(e => e == null); // Remove dead enemies

        // ✅ UI Update: Show **currently alive** enemies
        FindObjectOfType<UI>().SetEnemiesLeft(activeEnemies.Count);

        // ✅ CHECK IF ALL ENEMIES ARE DEAD
        if (waveInProgress && activeEnemies.Count == 0 && enemiesSpawned >= totalEnemiesInWave)
        {
            waveInProgress = false; // ✅ Stop current wave processing
            Debug.Log($"✅ All enemies from Wave {waveNumber} are dead! Preparing next wave...");
            Invoke(nameof(StartNextWave), 3f); // Delay before next wave
        }
    }

    void StartWave()
    {
        if (waveInProgress) return; // Prevent accidental overlap

        waveInProgress = true;
        Debug.Log($"🌊 Wave {waveNumber} Started!");

        totalEnemiesInWave = waveNumber * 4; // ✅ Ensures 4 → 8 → 12 → 16...
        enemiesSpawned = 0;
        activeEnemies.Clear();
        nextSpawnTime = Time.time;

        // ✅ UI Update
        FindObjectOfType<UI>().SetWaveNumber(waveNumber);
        FindObjectOfType<UI>().SetEnemiesLeft(0); // No enemies yet, start at 0
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
        {
            Debug.LogError("❌ No spawn points or enemy prefabs assigned!");
            return;
        }

        if (enemiesSpawned >= totalEnemiesInWave)
        {
            Debug.Log("⚠️ Attempted to spawn but wave limit reached.");
            return;
        }

        // 🔹 Pick a random enemy type & spawn point
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        activeEnemies.Add(newEnemy);
        enemiesSpawned++;

        Debug.Log($"👹 Spawned {enemyPrefab.name} at {spawnPoint.position} ({enemiesSpawned}/{totalEnemiesInWave})");

        // ✅ UI Update: Show how many are **alive on the map**
        FindObjectOfType<UI>().SetEnemiesLeft(activeEnemies.Count);
    }

    private void StartNextWave()
    {
        waveNumber++;
        totalEnemiesInWave = waveNumber * 4; // ✅ Increments by 4 every round
        enemiesSpawned = 0;
        waveInProgress = true;

        // ✅ UI Update
        FindObjectOfType<UI>().SetWaveNumber(waveNumber);
        FindObjectOfType<UI>().SetEnemiesLeft(0); // Start at 0 until enemies spawn
    }

    public int GetRemainingEnemies()
    {
        return activeEnemies.Count; // ✅ Return actual count of **alive** enemies
    }
    public int GetCurrentWave()
    {
        return waveNumber; // ✅ Return the current wave
    }

}
