using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

namespace game
{
    public class WaveManager : MonoBehaviour // Assited with AI
    {
        [Header("Initial Setup")]
        [Tooltip("Initial enemies in the scene")]
        [SerializeField] private List<Enemy> startingEnemies;

        [Header("Wave Settings")]
        [Tooltip("Prefab of the enemies.")]
        [SerializeField] private GameObject enemyPrefab;
        [Tooltip("Location of enemy spawn points.")]
        [SerializeField] private Transform[] spawnPoints;
        [Tooltip("Time before the next wave spawns.")]
        [SerializeField] private float timeBetweenWaves = 0.5f;

        [Header("Boss Settings")]
        [Tooltip("Prefab of boss.")]
        [SerializeField] private GameObject bossPrefab;
        [Tooltip("Number of waves before the boss.")]
        [SerializeField] private int wavesBeforeBoss = 2;

        [Header("UI")]
        [Tooltip("Text that displays the number of enemies left.")]
        [SerializeField] private TextMeshProUGUI enemyText;

        private int enemiesAlive;
        private int waveIndex = 0;
        private bool bossSpawned = false;

        private void OnEnable()
        {
            Enemy.OnEnemyKilled += HandleEnemyKilled;
        }

        private void OnDisable()
        {
            Enemy.OnEnemyKilled -= HandleEnemyKilled;
        }

        private void Start()
        {
            enemiesAlive = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
            UpdateEnemyCount();
        }

        // Lowers enemy count and checks if all enemies are dead
        private void HandleEnemyKilled(Enemy enemy)
        {
            enemiesAlive--;
            UpdateEnemyCount();
            if (enemiesAlive <= 0)
            {
                StartCoroutine(NextWave());
            }
        }

        // Starts the next wave by spawning enemies
        private IEnumerator NextWave()
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            if (!bossSpawned)
            {
                waveIndex++;
                if (waveIndex >= wavesBeforeBoss)
                {
                    SpawnBossWave();
                    SpawnEnemies(3);
                    yield break;
                }
                else
                {
                    SpawnEnemies(3 + waveIndex); // scaling difficulty
                }
            }
        }

        private void SpawnEnemies(int count)
        {
            enemiesAlive = count;
            UpdateEnemyCount();
            for (int i = 0; i < count; i++)
            {
                Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawn.position, spawn.rotation);
            }
        }

        private void SpawnBossWave()
        {
            bossSpawned = true;

            // Spawn boss
            Transform spawn = spawnPoints[0];
            Instantiate(bossPrefab, spawn.position, spawn.rotation);

            // Spawn backup enemies
            SpawnEnemies(2);

            enemiesAlive = 3;
            UpdateEnemyCount();
        }

        private void UpdateEnemyCount()
        {
            enemyText.text = "Enemies Left: " + enemiesAlive;
        }
    }
}
