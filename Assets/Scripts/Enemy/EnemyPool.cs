using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView enemyPrefab;
        private EnemyScriptableObject enemySO;

        public EnemyPool(EnemyView enemyPrefab, EnemyScriptableObject enemySO)
        {
            this.enemyPrefab = enemyPrefab;
            this.enemySO = enemySO;
        }

        public EnemyController GetEnemy()
        {
            EnemyController enemy = GetItem<EnemyController>();
            enemy.Reset();
            return enemy;
        }

        protected override EnemyController CreateItem<T>()
        {
            EnemyView view = Object.Instantiate(enemyPrefab);
            view.gameObject.SetActive(false);
            return new EnemyController(enemySO, view, this);
        }
    }
}