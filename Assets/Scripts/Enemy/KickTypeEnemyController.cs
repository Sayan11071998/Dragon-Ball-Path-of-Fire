namespace DragonBall.Enemy
{
    public class KickTypeEnemyController : BaseEnemyController
    {
        public KickTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override BaseEnemyModel CreateModel(EnemyScriptableObject enemyData)
        {
            return new KickTypeEnemyModel(
                enemyData.MaxHealth,
                enemyData.AttackDamage,
                enemyData.AttackCooldown
            );
        }
    }
}