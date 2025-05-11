using DragonBall.Enemy.EnemyData;
using DragonBall.Enemy.EnemyUtilities;
using DragonBall.Enemy.ParentMVC;

namespace DragonBall.Enemy.PowerBallEnemyMVC
{
    public class PowerBallFireTypeEnemyController : BaseEnemyController
    {
        public PowerBallFireTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override BaseEnemyModel CreateModel(EnemyScriptableObject enemyData)
        {
            return new PowerBallFireTypeEnemyModel(
                enemyData.MaxHealth,
                enemyData.AttackDamage,
                enemyData.AttackCooldown
            );
        }
    }
}