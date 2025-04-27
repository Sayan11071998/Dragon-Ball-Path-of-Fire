using DragonBall.Utilities;

namespace DragonBall.Enemy
{
    public class EnemyPool : GenericObjectPool<EnemyController>
    {
        private EnemyView enemyPrefab;

        public EnemyPool(EnemyView enemyPrefab) => this.enemyPrefab = enemyPrefab;

        public EnemyController GetEnemy() => GetItem<EnemyController>();

        protected override EnemyController CreateItem<U>() => new EnemyController(enemyPrefab);
    }
}