using DragonBall.Utilities;
using UnityEngine;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<BaseEnemyController>
    {
        private BaseEnemyView baseEnemyPrefab;
        private EnemyScriptableObject enemyScriptableObject;
        private EnemyType enemyType;

        public EnemyPool(BaseEnemyView enemyPrefabToSet, EnemyScriptableObject enemyScriptableObjectToSet, EnemyType enemyTypeToSet)
        {
            baseEnemyPrefab = enemyPrefabToSet;
            enemyScriptableObject = enemyScriptableObjectToSet;
            enemyType = enemyTypeToSet;
        }

        public BaseEnemyController GetEnemy()
        {
            BaseEnemyController enemy = GetItem<BaseEnemyController>();
            enemy.Reset();
            return enemy;
        }

        protected override BaseEnemyController CreateItem<T>()
        {
            BaseEnemyView view = Object.Instantiate(baseEnemyPrefab);
            view.gameObject.SetActive(false);
            return CreateEnemyController(view);
        }

        private BaseEnemyController CreateEnemyController(BaseEnemyView view)
        {
            switch (enemyType)
            {
                case EnemyType.KickType:
                    return new KickTypeEnemyController(enemyScriptableObject, view, this);
                case EnemyType.PowerBallFireType:
                    return new PowerBallFireTypeEnemyController(enemyScriptableObject, view, this);
                default:
                    Debug.LogError($"Unsupported enemy type: {enemyType}");
                    return null;
            }
        }
    }
}