using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.FlyingEnemyMVC
{
    public class FlyingTypeEnemyController : BaseEnemyController
    {
        public FlyingTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override BaseEnemyModel CreateModel(EnemyScriptableObject enemyData)
        {
            return new FlyingTypeEnemyModel(
                enemyData.MaxHealth,
                enemyData.AttackDamage,
                enemyData.AttackCooldown
            );
        }
    }
}