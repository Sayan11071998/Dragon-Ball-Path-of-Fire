using System.Collections.Generic;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPools = new Dictionary<EnemyType, EnemyPool>();

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> enemyConfigs)
        {
            foreach (var config in enemyConfigs)
            {
                EnemyType type = config.Key;
                EnemyView prefab = config.Value.Item1;
                EnemyScriptableObject so = config.Value.Item2;
                EnemyPool pool = new EnemyPool(prefab, so);
                enemyPools[type] = pool;
            }
        }

        public EnemyController SpawnEnemy(EnemyType type)
        {
            if (enemyPools.TryGetValue(type, out EnemyPool pool))
            {
                EnemyController enemy = pool.GetEnemy();
                enemy.EnemyView.gameObject.SetActive(true);
                return enemy;
            }
            else
            {
                Debug.LogError($"No pool for enemy type {type}");
                return null;
            }
        }
    }
}