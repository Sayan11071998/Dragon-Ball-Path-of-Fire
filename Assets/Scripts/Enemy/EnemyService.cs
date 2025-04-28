using System.Collections.Generic;

namespace DragonBall.Enemy
{
    public class EnemyService
    {
        private Dictionary<EnemyType, EnemyPool> enemyPools = new Dictionary<EnemyType, EnemyPool>();

        public EnemyService(Dictionary<EnemyType, (EnemyView, EnemyScriptableObject)> EnemyConfigs)
        {
            foreach (var config in EnemyConfigs)
            {
                EnemyType type = config.Key;
                EnemyView prefab = config.Value.Item1;
                EnemyScriptableObject so = config.Value.Item2;
                EnemyModel model = new EnemyModel(so.MaxHealth);
                EnemyPool pool = new EnemyPool(prefab, model);
                enemyPools[type] = pool;
            }
        }

        public EnemyController SpawnEnemy() { }
    }
}