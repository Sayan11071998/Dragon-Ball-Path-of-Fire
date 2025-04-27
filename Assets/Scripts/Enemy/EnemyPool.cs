using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView enemyPrefab;

        public EnemyPool(EnemyView enemyPrefab)
        {
            this.enemyPrefab = enemyPrefab;
        }

        public EnemyController GetEnemy() => GetItem<EnemyController>();

        protected override EnemyController CreateItem<U>()
        {
            // Instantiate the enemy view
            EnemyView newEnemyView = Object.Instantiate(enemyPrefab);
            newEnemyView.gameObject.SetActive(false);

            // Create the controller with the view
            return new EnemyController(newEnemyView);
        }

        public override void ReturnItem(EnemyController item)
        {
            item.ReturnToPool();
            base.ReturnItem(item);
        }
    }
}