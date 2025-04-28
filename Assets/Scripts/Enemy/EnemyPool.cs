using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView enemyPrefab;
        private EnemyModel enemyModel;

        public EnemyPool(EnemyView enemyPrefab, EnemyModel enemyModel)
        {
            this.enemyPrefab = enemyPrefab;
            this.enemyModel = enemyModel;
        }

        public EnemyController GetEnemy() => GetItem<EnemyController>();

        protected override EnemyController CreateItem<T>()
        {
            EnemyView view = Object.Instantiate(enemyPrefab);
            return new EnemyController(enemyModel, view, this);
        }
    }
}