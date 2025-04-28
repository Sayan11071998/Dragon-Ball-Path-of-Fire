using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView enemyPrefab;
        private EnemyScriptableObject enemyData;

        public EnemyPool(EnemyView enemyPrefab, EnemyScriptableObject enemyData)
        {
            this.enemyPrefab = enemyPrefab;
            this.enemyData = enemyData;
        }

        public EnemyController GetEnemy() => GetItem<EnemyController>();

        protected override EnemyController CreateItem<U>()
        {
            EnemyView newEnemyView = Object.Instantiate(enemyPrefab);
            newEnemyView.gameObject.SetActive(false);
            return new EnemyController(newEnemyView, enemyData);
        }

        public override void ReturnItem(EnemyController item)
        {
            item.ReturnToPool();
            base.ReturnItem(item);
        }
    }
}