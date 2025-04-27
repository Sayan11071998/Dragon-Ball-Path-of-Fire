using UnityEngine;
using DragonBall.Enemy;
using DragonBall.Core;

namespace DragonBall.Environment
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private bool spawnOnStart = true;
        [SerializeField] private int initialEnemyCount = 1;
        [SerializeField] private float spawnRadius = 2f;

        private void Start()
        {
            if (spawnOnStart)
            {
                SpawnEnemies();
            }
        }

        public void SpawnEnemies()
        {
            for (int i = 0; i < initialEnemyCount; i++)
            {
                // Calculate a random position within the radius
                Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

                // Spawn the enemy
                GameService.Instance.enemyService.SpawnEnemy(enemyType, spawnPosition);
            }
        }

        // Public method to spawn a single enemy
        public void SpawnEnemy()
        {
            GameService.Instance.enemyService.SpawnEnemy(enemyType, transform.position);
        }

        // Optional: Visualize spawn area in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}