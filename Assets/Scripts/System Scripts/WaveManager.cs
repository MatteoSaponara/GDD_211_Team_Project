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
        [Tooltip("Model of the boss on the throne that will be deactivated once the Boss wave starts.")]
        [SerializeField] private GameObject bossModel;

        [Header("Wave Settings")]
        [Tooltip("Prefab of the enemies.")]
        [SerializeField] private GameObject enemyPrefab;
        [Tooltip("Locations where normal enemies can spawn.")]
        [SerializeField] private Transform[] enemySpawnPoints;
        [Tooltip("Location where the boss spawns.")]
        [SerializeField] private Transform bossSpawnPoint;
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

        // Number of enemies alive
        private int enemiesAlive;
        // Number of the wave (-1)
        private int waveIndex = 0;
        // Determines if the boss has spawned
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
                if (waveIndex == 1)
                {
                    GameManager.instance.ChallengeManager.StartChallenge();
                }
                if (waveIndex >= wavesBeforeBoss)
                {
                    SpawnBossWave();
                    SpawnEnemies(3);
                    bossModel.gameObject.SetActive(false); // Deactivates boss model
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
            enemiesAlive += count;
            for (int i = 0; i < count; i++)
            {
                Transform spawn = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];
                Instantiate(enemyPrefab, spawn.position, spawn.rotation);
            }
            UpdateEnemyCount();
        }

        // Spawns boss wave
        private void SpawnBossWave()
        {
            bossSpawned = true;

            // Spawn boss
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

            // Spawn backup enemies
            SpawnEnemies(2);

            enemiesAlive += 1;
            UpdateEnemyCount();
        }

        // Updates Enemy UI
        private void UpdateEnemyCount()
        {
            enemyText.text = "Enemies Left: " + enemiesAlive;
        }
    }
}
