using System.Collections.Generic;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private EnemyPool enemyPool;
        private List<EnemyController> activeEnemies = new List<EnemyController>();

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> enemyConfigs)
        {
            enemyPool = new EnemyPool(enemyConfigs);
        }

        public void Update()
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                activeEnemies[i].Update();
            }
        }

        public EnemyController SpawnEnemy(EnemyType enemyType, Vector3 position)
        {
            EnemyController enemy = enemyPool.GetEnemy(enemyType);
            if (enemy != null)
            {
                enemy.EnemyView.transform.position = position;
                enemy.EnemyView.Initialize();
                activeEnemies.Add(enemy);
            }
            return enemy;
        }

        public void ReturnEnemyToPool(EnemyController enemy)
        {
            activeEnemies.Remove(enemy);
            enemyPool.ReturnItem(enemy);
        }

        public void ClearAllEnemies()
        {
            foreach (var enemy in activeEnemies)
            {
                enemy.EnemyView.DeactivateEnemy();
                enemyPool.ReturnItem(enemy);
            }
            activeEnemies.Clear();
        }
    }
}