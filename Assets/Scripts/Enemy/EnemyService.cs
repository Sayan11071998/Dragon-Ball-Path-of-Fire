using System.Collections.Generic;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPools = new Dictionary<EnemyType, EnemyPool>();

        public EnemyService(Dictionary<EnemyType, (BaseEnemyView, EnemyScriptableObject)> enemyData)
        {
            foreach (var item in enemyData)
            {
                EnemyType type = item.Key;
                BaseEnemyView prefab = item.Value.Item1;
                EnemyScriptableObject so = item.Value.Item2;
                EnemyPool pool = new EnemyPool(prefab, so, type);
                enemyPools[type] = pool;
            }
        }

        public BaseEnemyController SpawnEnemy(EnemyType type)
        {
            if (enemyPools.TryGetValue(type, out EnemyPool pool))
            {
                BaseEnemyController enemy = pool.GetEnemy();
                enemy.BaseEnemyView.gameObject.SetActive(true);
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