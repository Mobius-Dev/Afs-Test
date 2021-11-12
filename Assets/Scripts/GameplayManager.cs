namespace AFSInterview
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    public class GameplayManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject simpleTowerPrefab;
        [SerializeField] private GameObject advancedTowerPrefab;

        [Header("Settings")]
        [SerializeField] private Vector2 boundsMin;
        [SerializeField] private Vector2 boundsMax;
        [SerializeField] private float enemySpawnRate;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI enemiesCountText;

        private List<Enemy> enemies;
        private float enemySpawnTimer;
        private int score;

        private void Awake()
        {
            enemies = new List<Enemy>();
        }

        private void Update()
        {
            enemySpawnTimer -= Time.deltaTime;

            if (enemySpawnTimer <= 0f)
            {
                SpawnEnemy();
                enemySpawnTimer = enemySpawnRate;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Ground")))
                {
                    var spawnPosition = hit.point;
                    spawnPosition.y = simpleTowerPrefab.transform.position.y;

                    SpawnTower(spawnPosition, TowerType.Simple);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Ground")))
                {
                    var spawnPosition = hit.point;
                    spawnPosition.y = simpleTowerPrefab.transform.position.y;

                    SpawnTower(spawnPosition, TowerType.Advanced);
                }
            }

            scoreText.text = "Score: " + score;
            enemiesCountText.text = "Enemies: " + enemies.Count;
        }

        private void SpawnEnemy()
        {
            var position = new Vector3(Random.Range(boundsMin.x, boundsMax.x), enemyPrefab.transform.position.y, Random.Range(boundsMin.y, boundsMax.y));

            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity).GetComponent<Enemy>();
            enemy.OnEnemyDied += Enemy_OnEnemyDied;
            enemy.Initialize(boundsMin, boundsMax);

            enemies.Add(enemy);
        }

        private void Enemy_OnEnemyDied(Enemy enemy)
        {
            enemies.Remove(enemy);
            score++;
        }

        private void SpawnTower(Vector3 position, TowerType type)
        {
            GameObject newTowerPrefab;

            switch (type)
            {
                case TowerType.Simple:
                    newTowerPrefab = simpleTowerPrefab;
                    break;

                case TowerType.Advanced:
                    newTowerPrefab = advancedTowerPrefab;
                    break;

                default:
                    newTowerPrefab = simpleTowerPrefab;
                    break;
            }

            var tower = Instantiate(newTowerPrefab, position, Quaternion.identity).GetComponent<Tower>();

            tower.Initialize(enemies);
        }
    }
}