namespace DragonBall.Enemy
{
    public class KickTypeEnemyController : BaseEnemyController
    {
        public KickTypeEnemyController(EnemyScriptableObject enemySO, BaseEnemyView view, EnemyPool pool)
            : base(enemySO, view, pool) { }

        protected override void InitializeModel(EnemyScriptableObject enemySO)
        {
            baseEnemyModel = new KickTypeEnemyModel(enemySO.MaxHealth, enemySO.AttackDamage, enemySO.AttackCooldown);
        }
    }
}