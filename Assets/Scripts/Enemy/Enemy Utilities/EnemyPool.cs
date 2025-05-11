using UnityEngine;
using DragonBall.Enemy.ParentMVC;
using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.FinalBossEnemyMVC;
using DragonBall.Enemy.FlyingEnemyMVC;
using DragonBall.Enemy.KickEnemyMVC;
using DragonBall.Enemy.PowerBallEnemyMVC;
using DragonBall.Utilities;

namespace DragonBall.Enemy.EnemyUtilities
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

        private BaseEnemyController CreateEnemyController(BaseEnemyView viewToSet)
        {
            switch (enemyType)
            {
                case EnemyType.KickType:
                    return new KickTypeEnemyController(enemyScriptableObject, viewToSet, this);
                case EnemyType.PowerBallFireType:
                    return new PowerBallFireTypeEnemyController(enemyScriptableObject, viewToSet, this);
                case EnemyType.FlyingType:
                    return new FlyingTypeEnemyController(enemyScriptableObject, viewToSet, this);
                case EnemyType.FinalBossType:
                    return new FinalBossTypeEnemyController(enemyScriptableObject, viewToSet, this);
                default:
                    return null;
            }
        }
    }
}