namespace DragonBall.Enemy
{
    public class FinalBossTypeEnemyController : FlyingTypeEnemyController
    {
        public FinalBossTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new FinalBossTypeEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}