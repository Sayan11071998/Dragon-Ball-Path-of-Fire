using UnityEngine;
using System.Collections.Generic;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPools;
        private Dictionary<EnemyType, EnemyScriptableObject> enemyConfigs;
        private List<EnemyController> activeEnemies = new List<EnemyController>();

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> _enemyConfigs)
        {
            enemyPools = new Dictionary<EnemyType, EnemyPool>();
            enemyConfigs = new Dictionary<EnemyType, EnemyScriptableObject>();

            foreach (var kvp in _enemyConfigs)
            {
                EnemyType type = kvp.Key;
                (EnemyView prefab, EnemyScriptableObject config) = kvp.Value;

                enemyConfigs[type] = config;
                enemyPools[type] = new EnemyPool(prefab);
            }
        }

        public EnemyController SpawnEnemy(EnemyType enemyType, Vector3 position)
        {
            if (!enemyPools.ContainsKey(enemyType))
            {
                Debug.LogError($"Enemy type {enemyType} not found in enemy pools!");
                return null;
            }

            EnemyScriptableObject config = enemyConfigs[enemyType];
            EnemyPool pool = enemyPools[enemyType];

            // Get enemy from pool
            EnemyController enemy = pool.GetEnemy();

            // If the enemy is new (first time created), we need to initialize its model and view
            if (enemy.NeedsInitialization)
            {
                EnemyModel model = new EnemyModel(
                    config.EnemyType,
                    config.MaxHealth,
                    config.MovementSpeed,
                    config.DetectionRange,
                    config.AttackRange
                );

                enemy.SetModel(model);
                enemy.SetPool(pool);
            }

            // Initialize position
            enemy.Initialize(position);

            // Add to active enemies list
            activeEnemies.Add(enemy);

            return enemy;
        }

        public void ReturnToPool(EnemyController enemy)
        {
            activeEnemies.Remove(enemy);
            enemyPools[enemy.EnemyType].ReturnItem(enemy);
        }

        public void Update()
        {
            // Update all active enemies
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                activeEnemies[i].Update();
            }
        }

        public void HandleEnemyDeath(EnemyController enemy)
        {
            ReturnToPool(enemy);
        }
    }
}