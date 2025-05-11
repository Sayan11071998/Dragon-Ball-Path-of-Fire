using System.Collections.Generic;
using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.EnemyUtilities
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPool = new Dictionary<EnemyType, EnemyPool>();

        public EnemyService(Dictionary<EnemyType, (BaseEnemyView, EnemyScriptableObject)> _enemyData)
        {
            foreach (var item in _enemyData)
            {
                EnemyType enemyType = item.Key;
                BaseEnemyView enemyPrefab = item.Value.Item1;
                EnemyScriptableObject enemyScriptableObject = item.Value.Item2;
                EnemyPool pool = new EnemyPool(enemyPrefab, enemyScriptableObject, enemyType);
                enemyPool[enemyType] = pool;
            }
        }

        public BaseEnemyController SpawnEnemy(EnemyType enemyType)
        {
            if (enemyPool.TryGetValue(enemyType, out EnemyPool pool))
            {
                BaseEnemyController enemy = pool.GetEnemy();
                enemy.BaseEnemyView.gameObject.SetActive(true);
                return enemy;
            }
            else
            {
                return null;
            }
        }
    }
}