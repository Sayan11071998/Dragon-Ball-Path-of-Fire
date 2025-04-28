using UnityEngine;
using System.Collections.Generic;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPools;
        private List<EnemyController> activeEnemies = new List<EnemyController>();

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> enemyConfigs)
        {
            enemyPools = new Dictionary<EnemyType, EnemyPool>();
            foreach (var item in enemyConfigs)
            {
                EnemyType type = item.Key;
                EnemyView prefab = item.Value.Item1;
                EnemyScriptableObject data = item.Value.Item2;
                enemyPools[type] = new EnemyPool(prefab, data);
            }
        }

        public EnemyController SpawnEnemy(EnemyType enemyType, Vector3 position)
        {
            if (!enemyPools.ContainsKey(enemyType))
                return null;

            EnemyController enemy = enemyPools[enemyType].GetEnemy();
            enemy.Initialize(position);
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
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
                activeEnemies[i].Update();
        }
    }
}