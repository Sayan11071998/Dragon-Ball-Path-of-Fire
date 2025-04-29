using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<BaseEnemyController>
    {
        private BaseEnemyView enemyPrefab;
        private EnemyScriptableObject enemySO;
        private EnemyType enemyType;

        public EnemyPool(BaseEnemyView enemyPrefab, EnemyScriptableObject enemySO, EnemyType enemyType)
        {
            this.enemyPrefab = enemyPrefab;
            this.enemySO = enemySO;
            this.enemyType = enemyType;
        }

        public BaseEnemyController GetEnemy()
        {
            BaseEnemyController enemy = GetItem<BaseEnemyController>();
            enemy.Reset();
            return enemy;
        }

        protected override BaseEnemyController CreateItem<T>()
        {
            BaseEnemyView view = Object.Instantiate(enemyPrefab);
            view.gameObject.SetActive(false);

            // Create the appropriate enemy controller based on the type
            return CreateEnemyController(view);
        }

        private BaseEnemyController CreateEnemyController(BaseEnemyView view)
        {
            switch (enemyType)
            {
                case EnemyType.Buu:
                    return new BuuEnemyController(enemySO, view, this);
                // Add more cases for other enemy types here
                default:
                    Debug.LogError($"Unsupported enemy type: {enemyType}");
                    return null;
            }
        }
    }
}